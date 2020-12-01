$('#Get').click(function () {

    var test = $('#name option:selected').text();
    var test1 = test.val;
    $.getJSON("/Chart/GetDataToChart/Bench/01.01.0001%2000:00:00/12.27.2020%2000:00:00", function (data) {

        var categories = [];
        var weight = [];
        console.log("ciulsko");
        for (var i = 0; i < data.length; i++) {
            categories.push(data[i].trainingHistory.date);
            console.log(data[i]);
            weight.push(data[i].weight);
        }

        console.log(data[0].name);
        Highcharts.chart('container', {
            chart: {
                type: 'line'
            },
            title: {
                text: 'Monthly Average Temperature'
            },
            subtitle: {
                text: 'Source: WorldClimate.com'
            },
            xAxis: {
                categories: categories
            },
            yAxis: {
                title: {
                    text: 'Temperature (°C)'
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
                name: $('#name').val,
                data: weight
            }]
        });
    });
})
