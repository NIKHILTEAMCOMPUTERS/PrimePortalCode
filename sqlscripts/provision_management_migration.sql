-- =============================================================================
-- Provision Management Feature Migration
-- Description : Adds carry-forward tracking, provision status, history action
--               columns and performance indexes; updates sidebar icon for
--               Provision Management page.
-- Target DB   : RMS-Live (PostgreSQL)
-- Date        : 2026-05-24
-- Author      : PrimePortal Dev
-- Run order   : 1. Schema changes  2. Indexes  3. Page icon
-- Idempotent  : YES – all statements use IF NOT EXISTS / safe defaults
-- =============================================================================

BEGIN;

-- ---------------------------------------------------------------------------
-- 1. TABLE: contractbillingprovesion
--    New columns: carryforwardcount, carryforwardfromid, provisionstatus, documentno
-- ---------------------------------------------------------------------------

ALTER TABLE contractbillingprovesion
    ADD COLUMN IF NOT EXISTS carryforwardcount  INTEGER      NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS carryforwardfromid INTEGER      NULL,
    ADD COLUMN IF NOT EXISTS provisionstatus    VARCHAR(50)  NOT NULL DEFAULT 'Active',
    ADD COLUMN IF NOT EXISTS documentno        VARCHAR(100) NULL;

-- Backfill: any row that got NULL because it was inserted before the DEFAULT
-- was applied (edge-case guard; normally DEFAULT handles existing rows).
UPDATE contractbillingprovesion
SET    provisionstatus = 'Active'
WHERE  provisionstatus IS NULL;

-- ---------------------------------------------------------------------------
-- 2. TABLE: contractbillingprovisionhistory
--    New columns: actiontype, actionby, oldvalues, newvalues
--    (used to record Settle / CarryForward / Reverse / Edit actions)
-- ---------------------------------------------------------------------------

ALTER TABLE contractbillingprovisionhistory
    ADD COLUMN IF NOT EXISTS actiontype VARCHAR(100) NULL,
    ADD COLUMN IF NOT EXISTS actionby   VARCHAR(200) NULL,
    ADD COLUMN IF NOT EXISTS oldvalues  TEXT         NULL,
    ADD COLUMN IF NOT EXISTS newvalues  TEXT         NULL;

-- ---------------------------------------------------------------------------
-- 3. INDEXES
--    Improve query performance for the provision list and history lookups.
-- ---------------------------------------------------------------------------

-- contractbillingprovesion
CREATE INDEX IF NOT EXISTS idx_cbp_billingmonthyear
    ON contractbillingprovesion (billingmonthyear);

CREATE INDEX IF NOT EXISTS idx_cbp_provisionstatus
    ON contractbillingprovesion (provisionstatus);

CREATE INDEX IF NOT EXISTS idx_cbp_carryforwardfromid
    ON contractbillingprovesion (carryforwardfromid);

CREATE INDEX IF NOT EXISTS idx_cbp_carryforwardcount
    ON contractbillingprovesion (carryforwardcount);

-- contractbillingprovisionhistory
CREATE INDEX IF NOT EXISTS idx_cbph_provesionid
    ON contractbillingprovisionhistory (contractbillingprovesionid);

CREATE INDEX IF NOT EXISTS idx_cbph_actiontype
    ON contractbillingprovisionhistory (actiontype);

-- ---------------------------------------------------------------------------
-- 4. PAGE ICON
--    Set the sidebar icon for the Provision Management menu item.
-- ---------------------------------------------------------------------------

UPDATE page
SET    icon = '/assets/images/traffic_icon-2.svg'
WHERE  pageid = 30
  AND  (icon IS NULL OR icon = '');

-- ---------------------------------------------------------------------------
-- 5. TABLE: team_provision_tracking  (NEW – Team Pending Provision feature)
--    Side-car table linked to contractbillingprovesion; created on first action
--    for a provision (GetOrCreateTracking pattern).
-- ---------------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS team_provision_tracking (
    id                SERIAL       PRIMARY KEY,
    provision_id      INTEGER      NOT NULL,
    closer_date       TIMESTAMP    NULL,
    document_no       VARCHAR(100) NULL,
    billed_amount     NUMERIC(18,2) NOT NULL DEFAULT 0,
    remark            TEXT         NULL,
    is_deleted        BOOLEAN      NOT NULL DEFAULT FALSE,
    created_by        INTEGER      NOT NULL DEFAULT 0,
    created_date      TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,
    last_updated_by   INTEGER      NOT NULL DEFAULT 0,
    last_updated_date TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT team_provision_tracking_provision_id_fkey
        FOREIGN KEY (provision_id)
        REFERENCES contractbillingprovesion (contractbillingprovesionid)
        ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS idx_tpt_provision_id
    ON team_provision_tracking (provision_id);

-- ---------------------------------------------------------------------------
-- 6. TABLE: team_provision_history  (NEW – Team Pending Provision feature)
--    Audit trail for every action performed via the Team Pending Provision page.
-- ---------------------------------------------------------------------------

CREATE TABLE IF NOT EXISTS team_provision_history (
    id               SERIAL       PRIMARY KEY,
    team_tracking_id INTEGER      NULL,
    provision_id     INTEGER      NOT NULL,
    action_type      VARCHAR(50)  NOT NULL,
    action_by        VARCHAR(200) NOT NULL,
    old_values       TEXT         NULL,
    new_values       TEXT         NULL,
    remark           TEXT         NULL,
    is_deleted       BOOLEAN      NOT NULL DEFAULT FALSE,
    created_by       INTEGER      NOT NULL DEFAULT 0,
    created_date     TIMESTAMP    NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT team_provision_history_team_tracking_id_fkey
        FOREIGN KEY (team_tracking_id)
        REFERENCES team_provision_tracking (id)
        ON DELETE SET NULL
);

CREATE INDEX IF NOT EXISTS idx_tph_provision_id
    ON team_provision_history (provision_id);

CREATE INDEX IF NOT EXISTS idx_tph_team_tracking_id
    ON team_provision_history (team_tracking_id);

-- ---------------------------------------------------------------------------
-- 7. PAGE REGISTRATION  (Team Pending Provision menu item)
--    Insert only if a row with the same controller+action does not yet exist.
-- ---------------------------------------------------------------------------

INSERT INTO page (pagename, icon, moduleid, controllername, actionname, isactive)
SELECT 'Team Pending Provision',
       '/assets/images/traffic_icon-2.svg',
       3,
       'TeamPendingProvision',
       'Index',
       TRUE
WHERE NOT EXISTS (
    SELECT 1 FROM page
    WHERE controllername = 'TeamPendingProvision'
      AND actionname     = 'Index'
);

-- ---------------------------------------------------------------------------
-- Done
-- ---------------------------------------------------------------------------

COMMIT;
