$(document).on("click", ".phone-modal-btn", function (e) {
    e.preventDefault()
    let url = $(this).attr("href");
    fetch(url).then(response => response.text())
        .then(data => {
            $("#quick-view-modal .modal-dialog").html(data)
        })
    $("#quick-view-modal").modal("show")
    
})