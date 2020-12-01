$('#Get').click(function () {

    var name = $('#name option:selected').text();

    var dateFrom = $('#dateFrom option:selected').text();
    var partsFrom = dateFrom.split(/[.]/);
    var finalDateFrom = `${partsFrom[1]}.${partsFrom[0]}.${partsFrom[2]}`;

    var dateTo = $('#dateTo option:selected').text();
    var partsTo = dateTo.split(/[.]/);
    var finalDateTo = `${partsTo[1]}.${partsTo[0]}.${partsTo[2]}`;

    var test = '/Chart/GetDataToChart/' + name + '/' + dateFrom + '/' + dateTo;
    console.log(test);

    $.getJSON('/Chart/GetDataToChart/' + name + '/' + finalDateFrom + '/' + finalDateTo, function (data) {

        var categories = [];
        var weight = [];
        console.log("ciulsko");
        for (var i = 0; i < data.length; i++) {
            categories.push(String(data[i].trainingHistory.date).substr(0, 10));
            console.log(data[i]);
            weight.push(data[i].weight);
        }

        Highcharts.chart('container', {
            chart: {
                type: 'line'
            },
            title: {
                text: 'Progress chart'
            },
            subtitle: {
                text: ''
            },
            xAxis: {
                categories: categories
            },
            yAxis: {
                title: {
                    text: 'Weight'
                }
            },
            plotOptions: {
                line: {
                    dataLabels: {
                        enabled: true
                    },
                    enableMouseTracking: false
                }
            },
            series: [{
                name: $('#name option:selected').text(),
                data: weight
            }]
        });
    });
})
