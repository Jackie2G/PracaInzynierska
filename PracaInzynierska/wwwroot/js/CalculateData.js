﻿$('#Get').click(function () {

    var w = $('#weight').val();
    var r = $('#reps').val();
    var method = $('#method option:selected').text();
    var output = $('#output');
    var repMax;

    switch (method) {

        case 'Epley':
            repMax = parseFloat(w * (1 + r / 30)).toFixed(2);
            output.val(repMax);
            break;

        case 'Brzycki':
            repMax = parseFloat(w / (1.0278 - 0.0278 * r)).toFixed(2);
            output.val(repMax);
            break;

        case 'McGlothin':
            repMax = parseFloat(100 * w / (101.3 - 2.67123 * r)).toFixed(2);
            output.val(repMax);
            break;

        case 'Lombardi':
            repMax = parseFloat(w * Math.pow(r, 0.1)).toFixed(2);
            output.val(repMax);
            break;

        case 'Mayhew et ai.':
            repMax = parseFloat(100 * w / (52.2 + 41.9 * Math.pow(Math.E, -0.055 * r))).toFixed(2);
            output.val(repMax);
            break;

        case "O'Conner et ai.":
            repMax = parseFloat(w * (1 + r / 40)).toFixed(2);
            output.val(repMax);
            break;

        case 'Wathen':
            repMax = parseFloat(100 * w / (48.8 + 53.8 * Math.pow(Math.E, -0.075 * r))).toFixed(2);
            output.val(repMax);
            break;
    }
});

$('#Reg').click(function () {

    var name = $('#name option:selected').text();
    var expectedDate = $('#expectedDate').val();
    var outputreg = $('#outputreg');
    var partsDate = expectedDate.split(/[-]/);
    var finalDate = `${partsDate[1]}.${partsDate[2]}.${partsDate[0]}`;
    var test = '/Prediction/' + name + '/' + finalDate;
    console.log(test);

    $.getJSON('/Prediction/' + name + '/' + finalDate, function (data) {

        console.log(finalDate);
        console.log(name);
        //for (var i = 0; i < data.length; i++) {
        //    console.log(data[i]);
        //}
        var output = parseFloat(data.endResult).toFixed(2);
        outputreg.val(output);
    });
})