$(document).on("click", ".add-to-basket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");

    fetch(url)
        .then(response => {
            if (!response.ok) {
                toastr["error"]("Məhsul bitib!")
                return;
            }
            else {
                toastr["success"]("Məhsul səbətə əlavə edildi")
                return response.text();
            }
        }).then(html => {
            $("#addtobasket").html(html)
        })
})



$(document).on("click", ".phone-modal-btn", function (e) {
    e.preventDefault()
    let url = $(this).attr("href");
    fetch(url).then(response => response.text())
        .then(data => {
            $("#quick-view-modal .modal-dialog").html(data)
        })
    $("#quick-view-modal").modal("show")
    
})


