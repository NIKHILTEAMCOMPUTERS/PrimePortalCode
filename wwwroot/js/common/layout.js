$(document).ready(function () {
    searchableLinks();
    //$("#loader").hide();
});


function searchableLinks() {
    $.ajax({
        async: false,
        url: "/Base/GetLinks",
        type: "POST",
        cache: false,
        contentType: false,
        processData: false,
        success: function (result) {
            if (result != "") {
                $("#header-search").autocomplete({
                    source: jQuery.parseJSON(result),
                    select: function (e, ui) {
                        location.href = ui.item.the_link;
                    }
                });
            }
        },
        error: function (errorThrown) {
        }
    });
}
function toggleSidebarSize() {
    document.querySelector(".sidebar").classList.toggle("shrinked");
}
function postForm(path, params, method) {
    method = method || 'post';
    var form = document.createElement('form');
    form.setAttribute('method', method);
    form.setAttribute('action', path);
    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            var hiddenField = document.createElement('input');
            hiddenField.setAttribute('type', 'hidden');
            hiddenField.setAttribute('name', key);
            hiddenField.setAttribute('value', params[key]);
            form.appendChild(hiddenField);
        }
    }
    document.body.appendChild(form);
    form.submit();
}

function toggleColVisibility(index, input) {
    const column = datatable.column(index);
    column.visible(input.checked);
}
function switchView(type) {
    if (type === "list") {
        document.querySelectorAll("table").forEach(item => {
            item.classList.remove("card-view");
        })
        document.querySelector("#customers-grid-view-btn").classList.remove("active");
        document.querySelector("#customers-list-view-btn").classList.add("active");
    } else if (type === "grid") {
        document.querySelectorAll("table").forEach(item => {
            item.classList.add("card-view");
        })
        document.querySelector("#customers-grid-view-btn").classList.add("active");
        document.querySelector("#customers-list-view-btn").classList.remove("active");
    }
}