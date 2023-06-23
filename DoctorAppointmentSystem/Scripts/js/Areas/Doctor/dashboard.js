

function initCalendar() {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        
        themeSystem: 'bootstrap',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
        },

        initialView: 'listWeek',
        initialDate: Date.now(),
        navLinks: true, // can click day/week names to navigate views
        selectable: false,
        selectMirror: true,
        editable: false,
        dayMaxEvents: true, // allow "more" link when too many events
        firstDay: 1, //The day that each week begins (Monday=1)
        eventSources: [

            {
                url: '/Doctor/Home/GetDoctorSchedule',
                type: 'POST',
                success: function (data) { console.log(data) },
                error: function () {
                    alert('Ocorreu um erro ao retornar as Tarefas. Por favor, entre em contato conosco.');
                    console.log(MediaError)
                },
                color: 'blue',   // a non-ajax option
                textColor: 'black' // a non-ajax option
            }

        ],


    });

    calendar.render();
}




$(document).ready(function () {
    initCalendar();


});
