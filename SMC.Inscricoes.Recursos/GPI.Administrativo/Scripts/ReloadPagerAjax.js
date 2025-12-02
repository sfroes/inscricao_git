reloadPagerAjax = function () {
    var grid = new smc.datagrid($('[data-control="grid"]:visible'));
    grid.refresh();
}