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

$(function () {
    $(".comment").slice(0, 3).show();
    $("body").on('click touchstart', '.load-more', function (e) {
        e.preventDefault();
        $(".comment:hidden").slice(0, 3).slideDown();
        if ($(".comment:hidden").length == 0) {
            $(".load-more").css('visibility', 'hidden');
        }
        $('html,body').animate({
            scrollTop: $(this).offset().top
        }, 100);
    });
});


$(function () {
    $(".single-product").slice(0, 12).show();
    $("body").on('click touchstart', '.load-more', function (e) {
        e.preventDefault();
        $(".single-product:hidden").slice(0, 12).slideDown();
        if ($(".single-product:hidden").length == 0) {
            $(".load-more").css('visibility', 'hidden');
        }
        $('html,body').animate({
            scrollTop: $(this).offset().top
        }, 100);
    });
});

/*-------------------------------------
    --> Range Slider
---------------------------------------*/
//$(function () {
//    $("#price-range").slider({
//        step: 500,
//        range: true,
//        min: 0,
//        max: 20000,
//        values: [0, 20000],
//        slide: function (event, ui) { $("#priceRange").val(ui.values[0] + " - " + ui.values[1]); }
//    });
//    $("#priceRange").val($("#price-range").slider("values", 0) + " - " + $("#price-range").slider("values", 1));

//});