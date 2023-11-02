// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var clickedSongId = 0;
var clickedArtistId = 0;
var clickedUserId = 0;


//this is a function to delete a selected song from the admin song management inside a table
$(document).ready(function () {
    $("#deleteSong").click(function () {
        if (confirm("Are you sure you want to delete this song?")) {
            //var token = $('input[name="__RequestVerificationToken"]').val(); // Get the token value
            $.ajax({
                url: '/AdminSong/DeleteSong',
                type: 'POST',
                data: {
                    "id": clickedSongId
                    
                },
                success: function (result) {
                    window.location.reload();
                }
            })
        }
    })

    //routes to the update song page
    $("#updateSong").click(function () {
        window.location.href = "/AdminSong/UpdateSong/" + clickedSongId;
    });

    //this will be used to delete the selected artist from the admin artist management
    $("#deleteArtist").click(function () {
        if (confirm("Are you sure you want to delete this artist?")) {
            $.ajax({
                url: '/AdminSong/DeleteArtist',
                type: 'POST',
                data: {
                    "id": clickedArtistId
                },
                success: function (result) {
                    window.location.reload();
                }
            })

        }
    })

//routes to the update artist page
    $("#updateArtist").click(function () {
        window.location.href = "/AdminSong/UpdateArtist/" + clickedArtistId;
    })
    //views the song that is allocated to that artist
    $("#viewSongs").click(function () {
        window.location.href = "/AdminSong/SongManagement/" + clickedArtistId;
    })

    //this is for the user management stuffs
    //first, this will delete the user
    $("#deleteUser").click(function () {
        var token = $('input[name="__RequestVerificationToken"]').val(); // Get the token value
        if (confirm("Are you sure you want to delete this user?")) {
            $.ajax({
                url: '/AdminUser/Delete',
                type: 'POST',
                headers: {
                    "RequestVerificationToken": token
                },
                data: {
                    "id": clickedUserId
                },
                error: function() {
                    alert("Unable to delete user")
                },
                success: function (result) {
                    alert("User has been deleted successfully");
                    window.location.reload();
                }
            })
        }
    })

    //routes to the update user page based on clicked user
    $("#updateUser").click(function () {
        window.location.href = "/AdminUser/Update/" + clickedUserId;
    });
})

//this function will listen to the click event on the table row and highlight the row
function onSongClick(id) {
    //listens to the song iD that was clicked and stores it in a variable
    clickedSongId = id;
    //enables the buttons
    $("#deleteSong").prop("disabled", false);
    $("#updateSong").prop("disabled", false);
    $(document).ready(function () {
        $("#songTable tr").removeClass("bg-secondary text-white");
        $("#" + id).addClass("bg-secondary text-white");
        
    });
    console.log("clicked " + clickedSongId)
}

//this function will listen to the click event on the table row and highlight the row from the Artist table
function onArtistClick(id) {
    //listens to the song iD that was clicked and stores it in a variable
    clickedArtistId = id;
    //enables the buttons
    $("#deleteArtist").prop("disabled", false);
    $("#updateArtist").prop("disabled", false);
    $("#viewSongs").prop("disabled", false);
    $(document).ready(function () {
        $("#artistTable tr").removeClass("bg-secondary text-white");
        $("#" + id).addClass("bg-secondary text-white");

    });
    console.log("clicked " + clickedArtistId)
}

//this function will listen to the click event on the table row and highlight the row from the Users table
function onUserClick(id) {
    clickedUserId = id;
    $("#deleteUser").prop("disabled", false);
    $("#updateUser").prop("disabled", false);
    $(document).ready(function () {
        $("#userTable tr").removeClass("bg-secondary text-white");
        $("#" + id).addClass("bg-secondary text-white");
    });
    console.log("clicked " + clickedUserId);
}