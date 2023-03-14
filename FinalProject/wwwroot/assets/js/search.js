let search__link = "/phone/GetSearch?search=";
let timeout = null;

$(document).on("keyup", ".searchInput", function (e) {
    clearTimeout(timeout);
    timeout = setTimeout(() => {
        let values = e.target.value.split(" ").join("%25")
        let newLink = search__link + values;

        fetch(newLink)
            .then(res => res.text())
            .then(data => {
                $("#search__holder").html(data);
            })

    }, 500)
})