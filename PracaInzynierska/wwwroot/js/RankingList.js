$('#Get').click(function () {
    loadDataTable();
});

function loadDataTable() {
    var exercise = $('#exercise option:selected').text();
    var gender = $('#gender option:selected').text();
    var weightCategory = $('#weightCategory').find(":selected").text();

    if (weightCategory == "120+") {
        weightCategory = "120up";
    }
    else if (weightCategory == "84+") {
        weightCategory = "84up";
    }
    else {
        weightCategory = weightCategory;
    }

    var getUrl = ('/Ranking/GetUsersBestExercises/' + exercise + '/' + gender + '/' + weightCategory).toString();
    console.log(getUrl);

    dataTable = $('#Ranking_load').DataTable({
        "destroy": true,
        "ajax": {
            "cache": true,
            "url": getUrl,
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "nick", "width": "20%" },
            { "data": "gender", "width": "10%" },
            { "data": "weightCategory", "width": "10%" },
            { "data": "exerciseName", "width": "20%" },
            { "data": "series", "width": "10%" },
            { "data": "reps", "width": "10%" },
            { "data": "weight", "width": "20%" },
        ],
        "language": {
            "emptyTable": "No rows in table"
        },
        "width": "100%"
    });
}
