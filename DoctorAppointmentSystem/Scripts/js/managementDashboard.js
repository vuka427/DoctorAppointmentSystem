




//load data to chart
function LoadDataToChart() {
   
           

            $.ajax({
                type: "GET",
                url: "/Admin/Manage/GetChartApm",
                data: "",
                dataType: "json",
                encode: true,
            }).done(function (data) {
                console.log(data);
                if (data.error == 1) {
                }
                if (data.error == 0) {
                    const ctx = document.getElementById('myChart');

                    new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: ['Monday', 'Tuesday ', 'Wednesday', 'Thursday ', 'Friday ', 'Saturday', 'Sunday'],
                            datasets: [{
                                label: 'Appointments',
                                data: data.datachart,
                                borderWidth: 1
                            }]
                        },
                        options: {
                            scales: {
                                y: {
                                    beginAtZero: true
                                }
                            }
                        }
                    });


                }
            });
}



$("document").ready(function () {
    LoadDataToChart();

});