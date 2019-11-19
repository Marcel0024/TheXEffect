document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        plugins: ['interaction', 'dayGrid'],
        height: 'parent',
        header: {
            left: 'prev,next today',
            center: '',
            right: 'title'
        },
        defaultView: 'dayGridMonth',
        editable: false,
        allDayDefault: true,
        selectable: false,
        eventSources: [
            getEvents
        ],
        eventClick: function (info) {
            toggleEvent(
                info.event,
                () => calendar.refetchEvents()
            );
        },
        dateClick: function (info) {
            toggleEvent(
                {
                    title: 'X',
                    start: info.dateStr
                },
                () => calendar.refetchEvents()
            );
        }
    });

    if (authenticated === true) {
        calendar.addEventSource('/api/calendar/getevents');
    }

    calendar.render();

    var center = document.getElementsByClassName('fc-center')[0];
    center.innerHTML = '<h1 style="color:#343a40">' + goal + '</h1>';
});

function getEvents(fetchInfo, successCallback, failureCallback) {
    var events = localStorage.getItem("thexeffectevents");

    if (events) {
        if (authenticated === true) {
            saveEventsToServer(events);

            successCallback(JSON.parse("[]"));
        }
        else {
            successCallback(JSON.parse(events));
        }
    } else {
        if (authenticated === true) {
            successCallback(JSON.parse("[]"));
        } else {
            localStorage.setItem("thexeffectevents", "[]");
            successCallback(JSON.parse("[]"));
        }
    }
}

function saveEventsToServer(events) {
    $.ajax({
        url: '/api/calendarevents/addmultiple',
        method: 'post',
        data: JSON.stringify({
            events: JSON.parse(events)
        }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            localStorage.removeItem("thexeffectevents");
            location.reload();
        }
    });
}

function toggleEvent(event, successCallback) {
    if (authenticated === true) {
        $.ajax({
            url: '/api/calendarevents/toggle',
            method: 'post',
            data: JSON.stringify({
                start: moment(event.start).format("YYYY-MM-DD")
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                successCallback();
            }
        });
    } else {
        toggleFromStorage(event, successCallback);
    }
}

function toggleFromStorage(event, callback) {
    var events = JSON.parse(localStorage.getItem("thexeffectevents"));

    var any = events.filter(function (el) {
        return moment(el.start).isSame(moment(event.start));
    });

    // remove if contains
    if (any.length > 0) {
        events = events.filter(function (el) {
            return !(moment(el.start).isSame(moment(event.start)));
        });
    }
    else {
        events.push({
            title: event.title,
            start: event.start
        });
    }

    localStorage.setItem("thexeffectevents", JSON.stringify(events));

    callback();
}