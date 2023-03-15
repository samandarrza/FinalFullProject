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

$(document).on("click", ".close-items", function (e) {
    e.preventDefault();
    let link = $(this).attr("href");

    fetch(link)
        .then(response => {
            if (!response.ok) {
                Swal.fire({
                    title: 'Error!',
                    text: 'This product is out of stock',
                    icon: 'error',
                    confirmButtonText: 'Ok'
                })
                throw new Error("something went wrong");
                return;
            }
            return response.text();
        })
        .then(data => {
            console.log(data)
            $("#addtobasket").html(data);
        })
        .catch(error => {
            console.log(error)
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