var dataTable;

$(document).ready(function () {
    loadDataTable();
    $('#datepicker').datepicker({
        dateFormat: 'dd/mm/yy'
    });
    //loadDataTableData();
});

function loadDataTable() {
    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/Training/getall/",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "trainingHistory.date", "width": "10%",
                "render": function (data, type, row) {
                    return String(data).substr(0, 10);
                }
            },
            { "data": "name", "width": "20%" },
            { "data": "series", "width": "10%" },
            { "data": "reps", "width": "10%" },
            { "data": "weight", "width": "10%" },
            { "data": "done", "width": "10%" },
            { "data": "description", "width": "10%" },
            { "data": "currentPr", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                        <a href="/Training/UpsertExercise?id=${data}" class='btn btn-light mr-1; width:70px;'>
                            <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-pencil-square" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                <path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456l-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>
                                <path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5v11z"/>
                            </svg>
                        </a>
                        &nbsp;
                        <a class='btn btn-light mr-1' width:70px;'
                            onclick=Delete('/Training/Delete?id='+${data})>
                            <svg width="1em" height="1em" viewBox="0 0 16 16" class="bi bi-trash-fill" fill="currentColor" xmlns="http://www.w3.org/2000/svg">
                                 <path fill-rule="evenodd" d="M2.5 1a1 1 0 0 0-1 1v1a1 1 0 0 0 1 1H3v9a2 2 0 0 0 2 2h6a2 2 0 0 0 2-2V4h.5a1 1 0 0 0 1-1V2a1 1 0 0 0-1-1H10a1 1 0 0 0-1-1H7a1 1 0 0 0-1 1H2.5zm3 4a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7a.5.5 0 0 1 .5-.5zM8 5a.5.5 0 0 1 .5.5v7a.5.5 0 0 1-1 0v-7A.5.5 0 0 1 8 5zm3 .5a.5.5 0 0 0-1 0v7a.5.5 0 0 0 1 0v-7z" />
                            </svg>
                        </a>
                        </div>`;
                }, "width": "10%"
            }
        ],
        "language": {
            "emptyTable": "Add exercises to create new training day"
        },
        "width": "100%"
    });
}

//function loadDataTableData() {
//    dataTable = $('#DT_load').DataTable({
//        "ajax": {
//            "url": "Training/SetData?data=17%2F09%2F2020/",
//            "type": "GET",
//            "datatype": "json"
//        },
//        "columns": [
//            { "data": "name", "width": "30%" },
//            { "data": "series", "width": "10%" },
//            { "data": "reps", "width": "10%" },
//            { "data": "weight", "width": "10%" },
//            { "data": "done", "width": "10%" },
//            {
//                "data": "id",
//                "render": function (data) {
//                    return `<div class="text-center">
//                        <a href="/Training/UpsertExercise?id=${data}" class='btn btn-success text-white' style='cursor:pointer; width:70px;'>
//                            Edit
//                        </a>
//                        &nbsp;
//                        <a class='btn btn-danger text-white' style='cursor:pointer; width:70px;'
//                            onclick=Delete('/Training/Delete?id='+${data})>
//                            Delete
//                        </a>
//                        </div>`;
//                }, "width": "30%"
//            }
//        ],
//        "language": {
//            "emptyTable": "Add exercises to create new training day"
//        },
//        "width": "100%"
//    });
//}

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