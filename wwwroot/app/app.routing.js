"use strict";
var router_1 = require('@angular/router');
var home_component_1 = require('./home.component');
var about_component_1 = require('./about.component');
var login_component_1 = require('./login.component');
var page_not_found_component_1 = require('./page-not-found.component');
var item_detail_edit_component_1 = require('./item-detail-edit.component');
var item_detail_view_component_1 = require('./item-detail-view.component');
var appRoutes = [
    {
        path: '',
        component: home_component_1.HomeComponent
    },
    {
        path: 'home',
        redirectTo: ''
    },
    {
        path: 'about',
        component: about_component_1.AboutComponent
    },
    {
        path: 'login',
        component: login_component_1.LoginComponent
    },
    {
        path: 'item/edit/:id',
        component: item_detail_edit_component_1.ItemDetailEditComponent
    },
    {
        path: 'item/view/:id',
        component: item_detail_view_component_1.ItemDetailViewComponent
    },
    {
        path: '**',
        component: page_not_found_component_1.PageNotFoundComponent
    }
];
exports.AppRoutingProviders = [];
exports.AppRouting = router_1.RouterModule.forRoot(appRoutes);

//# sourceMappingURL=app.routing.js.map
