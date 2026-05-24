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
--    New columns: carryforwardcount, carryforwardfromid, provisionstatus
-- ---------------------------------------------------------------------------

ALTER TABLE contractbillingprovesion
    ADD COLUMN IF NOT EXISTS carryforwardcount  INTEGER      NOT NULL DEFAULT 0,
    ADD COLUMN IF NOT EXISTS carryforwardfromid INTEGER      NULL,
    ADD COLUMN IF NOT EXISTS provisionstatus    VARCHAR(50)  NOT NULL DEFAULT 'Active';

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
-- Done
-- ---------------------------------------------------------------------------

COMMIT;
