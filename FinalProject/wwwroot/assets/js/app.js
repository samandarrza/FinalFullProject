$(document).on("click", ".add-to-basket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");

    fetch(url)
        .then(response => {
            if (!response.ok) {
                toastr["error"]("Product is out of stock")
                return;
            }
            else {
                toastr["success"]("Product has been added")
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


