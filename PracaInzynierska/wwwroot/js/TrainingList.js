﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
    $('#datepicker').datepicker({
        dateFormat: 'dd/mm/yy'
    });
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/Training/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "30%" },
            { "data": "series", "width": "10%" },
            { "data": "reps", "width": "10%" },
            { "data": "weight", "width": "10%" },
            { "data": "done", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Training/UpsertExercise?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
                            Edit
                        </a>
                        &nbsp;
                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
                            onclick=Delete('/Training/Delete?id='+${data})>
                            Delete
                        </a>
                        </div>`;
                }, "width": "30%"
            }
        ],
        "language": {
            "emptyTable": "Add exercises to create new training day"
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal({
        title: "Are you sure?",
        text: "Once deleted, you will not be able to recover",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}