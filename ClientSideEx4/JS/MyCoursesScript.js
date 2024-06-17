$(document).ready(function () {
    RenderUserCourses();
    $("#RatingRangeBTN").click(GetByRatingRange);
    $("#clearFilters").click(clearFilters);
    $("#DurationRangeBTN").click(GetByDurationRange);
});
//-------------------------------------------------------//
//--------------------Render User Course-----------------//
//-------------------------------------------------------//
function RenderUserCourses() {
    let api = `https://localhost:7020/api/UserCourse` + localStorage.getItem("loggedUser"); // check about it
    ajaxCall("GET", api, "", getSCBF, getECBF)
}

function getSCBF(result) {
    RenderCourses(result);
    console.log(result);
}

function getECBF(err) {
    
    console.log(err);

}

function RenderCourses(data) {
    const container = document.getElementById('containerCourses');
    container.innerHTML = "";
    for (let course of data) {
        const courseDiv = document.createElement('div');
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
    }
}

//-------------------------------------------------------//
//---------------------Rating Range----------------------//
//-------------------------------------------------------//

function GetByRatingRange() {
    const minRating = $("#minRating").val();
    const maxRating = $("#maxRating").val();
    if (minRating != "" && maxRating != "") {

        if (!isNaN(minRating) && !isNaN(maxRating)) {
            let api = `https://localhost:7020/api/UserCourse/GetByRatingRange/${localStorage.getItem("loggedUser")}?minRating=${minRating}&maxRating=${maxRating}`;
            ajaxCall("GET", api, "", ratingSCBF, ratingECBF);
        }
        else
            alert("Please enter only numbers")
    }
    else
        alert("Please enter values")
}
function ratingSCBF(data) {
    alert("Filter by rating")
    RenderCourses(data);
}

function ratingECBF(err) {
    console.log(err);
}
//-------------------------------------------------------//
//---------------------Rating Range----------------------//
//-------------------------------------------------------//

function GetByDurationRange() {
    const minDuration = $("#minDuration").val();
    const maxDuration = $("#maxDuration").val();
    if (minDuration != "" && maxDuration != "")
    {

        if (!isNaN(minDuration) && !isNaN(maxDuration)) {
            let api = `https://localhost:7020/api/UserCourse/GetByDurationRange/${localStorage.getItem("loggedUser")}?minDuration=${minDuration}&maxDuration=${maxDuration}`
            ajaxCall("GET", api, "", durationSCBF, durationECBF);
        }
        else
            alert("Please enter only numbers")
    }
    else
        alert("Please enter values")
}

function durationSCBF(data) {
    alert("Filter by duration")
    RenderCourses(data);
}

function durationECBF(err) {
    console.log(err);
}
//-------------------------------------------------------//
//---------------------Clear Filter----------------------//
//-------------------------------------------------------//
function clearFilters() {
    document.getElementById('minDuration').value = "";
    document.getElementById('maxDuration').value = "";
    document.getElementById('minRating').value = "";
    document.getElementById('maxRating').value = "";
    RenderUserCourses();
}