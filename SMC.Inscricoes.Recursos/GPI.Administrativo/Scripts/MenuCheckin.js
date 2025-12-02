$(".smc-gpi-menu-toogle").click(function (event) {
    event.preventDefault();

    $(".smc-gpi-menu").toggleClass("show");
    $(".smc-gpi-menu-backdrop").toggleClass("hidden");

    setTimeout(function () {
        $(".smc-gpi-menu").toggleClass("open");
    }, 1);
});

$(".smc-gpi-menu-backdrop").click(function (event) {
    event.preventDefault();

    $(".smc-gpi-menu").toggleClass("open");
    $(".smc-gpi-menu-backdrop").toggleClass("hidden");

    setTimeout(function () {
        $(".smc-gpi-menu").toggleClass("show");
    }, 200);

});

$(".smc-gpi-menu").click(function (event) {
    $(".smc-gpi-menu").toggleClass("open");
    $(".smc-gpi-menu-backdrop").toggleClass("hidden");

    setTimeout(function () {
        $(".smc-gpi-menu").toggleClass("show");
    }, 200);

});

