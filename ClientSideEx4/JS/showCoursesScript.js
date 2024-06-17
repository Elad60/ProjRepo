let courses; // Global variable to store courses data

$(document).ready(function () {
    $("#editDiv").hide(); // Hide edit form initially
    let api = `https://localhost:7020/api/Course`; // API endpoint to fetch courses
    ajaxCall("GET", api, "", getSCBF, getECBF); // Initial AJAX call to fetch courses

    // Event handler for save button click
    $('#saveBtn').on('click', function () {
        submitEditForm(); // Call function to submit edit form
    });
});

// Callback function for successful AJAX call to fetch courses
function getSCBF(coursesData) {
    console.log(coursesData);
    courses = coursesData; // Store courses data globally
    try {
        // Initialize DataTable with fetched data
        $('#coursesTable').DataTable({
            data: coursesData, // Data to populate in DataTable
            pageLength: 5, // Number of rows per page
            columns: [
                {
                    // Render edit button with course ID as data attribute
                    render: function (data, type, row, meta) {
                        let dataCourse = "data-courseId='" + row.id + "'";
                        return "<button type='button' class='editBtn btn btn-success' " + dataCourse + "> Edit </button>";
                    }
                },
                { data: "title" }, // Display course title
                { data: "rating" }, // Display course rating
                { data: "numberOfReviews" }, // Display number of reviews
                { data: "duration" }, // Display course duration
                { data: "instructorsId" }, // Display instructor ID
                {
                    // Render checkbox for isActive status
                    data: "isActive",
                    render: function (data, type, row, meta) {
                        return data ? '<input type="checkbox" checked disabled="disabled" />' : '<input type="checkbox" disabled="disabled"/>';
                    }
                },
            ],
        });
        buttonEvents(); // Set up event handlers for buttons in DataTable rows
    } catch (err) {
        console.error(err); // Log any errors encountered
    }
}

// Callback function for error in AJAX call
function getECBF(err) {
    console.error(err); // Log error message
}

// Set up event handlers for buttons in DataTable rows
function buttonEvents() {
    $('#coursesTable').on('click', '.editBtn', function () {
        let courseId = $(this).attr('data-courseId'); // Extract course ID from clicked button
        markSelected(this); // Highlight selected row in DataTable
        $("#editDiv").show(); // Display edit form
        $("#courseTitle").prop("disabled", false); // Enable editing of course title
        $("#courseIsActive").prop("disabled", false); // Enable editing of isActive checkbox
        populateFields(courseId); // Populate edit form fields with selected course data
    });
}

// Highlight selected row in DataTable
function markSelected(btn) {
    $("#coursesTable tr").removeClass("selected"); // Remove selected class from all rows
    let row = $(btn).closest('tr'); // Find closest row containing clicked button
    row.addClass('selected'); // Add selected class to highlight row
}

// Populate edit form fields with data of selected course
function populateFields(courseId) {
    console.log("Populate fields for course ID: " + courseId);
    let course = courses.find(c => c.id == courseId); // Find course object by ID in courses array
    if (course) {
        $("#courseId").val(course.id); // Set course ID in form field
        $("#courseTitle").val(course.title); // Set course title in form field
        $("#courseIsActive").prop("checked", course.isActive); // Set isActive checkbox based on course status
    }
}

// Submit edited course data to backend
function submitEditForm() {
    let courseId = $("#courseId").val(); // Get course ID from form field

    // Find the course in the courses array based on the courseId
    let originalCourse = courses.find(c => c.id == courseId);

    if (originalCourse) {
        // Create the editedCourse object and fill all fields
        let editedCourse = {
            id: originalCourse.id,
            title: $("#courseTitle").val() || originalCourse.title,
            url: originalCourse.url,
            rating: originalCourse.rating,
            numberOfReviews: originalCourse.numberOfReviews,
            instructorsId: originalCourse.instructorsId,
            imageReference: originalCourse.imageReference,
            duration: originalCourse.duration,
            lastUpdate: originalCourse.lastUpdate,
            isActive: $("#courseIsActive").is(":checked") || originalCourse.isActive
        };

        console.log("Submitting edited course:", editedCourse); // Log edited course data

        let api = `https://localhost:7020/api/Course/userUpdate`; // API endpoint for updating course

        ajaxCall("PUT", api, JSON.stringify(editedCourse), function (result) {
            console.log("Course updated successfully:", result); // Log success message
            updateIsActive(editedCourse.id, editedCourse.isActive);
        

            $("#editDiv").hide(); // Hide edit form after successful update
        }, function (error) {
            console.error("Error updating course:", error); // Log error message if update fails
        });
    } else {
        console.error("Course not found in courses array."); // Log error if course not found
    }
}
function updateIsActive(courseId, updatedIsActive) {
    let api = `https://localhost:7020/api/Course/IsActiveUpdate?courseId=${courseId}`;

    ajaxCall("PUT", api, JSON.stringify(updatedIsActive), function (result) {
        alert("Course has been edited succesfully");
        console.log("IsActive status updated successfully:", result);
        location.reload();
    }, function (error) {
        console.error("Error updating IsActive status:", error);
    });
}




// Generic function for making AJAX calls
function ajaxCall(method, url, data, successCallback, errorCallback) {
    $.ajax({
        type: method, // HTTP method (GET, POST, PUT, DELETE, etc.)
        url: url, // URL to make the request to
        data: data, // Data to send in the request body
        contentType: "application/json; charset=utf-8", // Content type of the request
        dataType: "json", // Expected data type of the response
        success: successCallback, // Callback function for successful response
        error: errorCallback, // Callback function for error response
        async: true // Asynchronous request (default: true)
    });
}
