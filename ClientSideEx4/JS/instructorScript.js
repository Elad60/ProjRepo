$(document).ready(function () {
    GetInstructors();
    $("#backBTN").click(GetInstructors);

})

function GetInstructors() {
    let api = `https://localhost:7020/api/Instructor`;
    ajaxCall("GET", api, "", getISCBF, getIECBF);
}

function getISCBF(result) {
    renderInstructors(result);
    console.log(result);
}

function getIECBF(err) {
    console.log(err);

}

function renderInstructors(data) {
    document.getElementById('backBTN').style.display = 'none';
    document.getElementById('containerInstructorCourses').innerHTML = '';
    const container = document.getElementById('containerInstructors');
    for (let instructor of data) {
        const instructorDiv = document.createElement('div');
        instructorDiv.id = "courseDiv";
        const html = `
                        <img src="${instructor.image}" alt="${instructor.title}">
                        <h2>${instructor.name}</h2>
                        <p>Job Title: ${instructor.jobTitle}</p>
                                   `;
        instructorDiv.innerHTML = html;
        let btn = document.createElement('button');
        btn.innerText = 'Show more courses of this instructor';
        btn.onclick = function () {

            let api = `https://localhost:7020/api/Instructor/` + instructor.id;
            ajaxCall("GET", api, "", getICSCBF, getICECBF);


        }
        instructorDiv.appendChild(btn);
        container.appendChild(instructorDiv);
    }
}

function getICSCBF(result) {
    RenderInstructorCourses(result);
    console.log(result);
}

function getICECBF(err) {
    console.log(err);

}
function RenderInstructorCourses(courses) {
    document.getElementById('containerInstructors').innerHTML = '';
    const container = document.getElementById('containerInstructorCourses');

    //btn back to main
    document.getElementById('backBTN').style.display = 'block';
    //render courses
    let name;
    for (let course of courses) {
        const courseDiv = document.createElement('div');
        const title = document.createElement('h1');
        title.textContent = localStorage.getItem(course.instructorsId) + ' Courses';
        courseDiv.appendChild(title);
        courseDiv.id = "courseDiv";
        const html = `
                        <img src="${course.imageReference}" alt="${course.title}">
                        <h2>${course.title}</h2>
                        <p>Instructor: ${localStorage.getItem(course.instructorsId)}</p>
                        <p>Rating: ${course.rating.toFixed(2)}</p>
                        <p>Number of Reviews: ${course.numberOfReviews}</p>
                        <p>Last Update Date: ${course.lastUpdate}</p>
                        <p>Duration: ${course.duration.toFixed(2)}</p>
                        <a href="https://udemy.com${course.url}" target="_blank">View Course</a>
                                   `;
        courseDiv.innerHTML = html;
        container.appendChild(courseDiv);
        name = localStorage.getItem(course.instructorsId);
    }

    //title
    const titleDiv = document.getElementById('title');
    titleDiv.textContent = name;
}