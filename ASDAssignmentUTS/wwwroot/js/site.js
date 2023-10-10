// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


var clickedSongId = 0;


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