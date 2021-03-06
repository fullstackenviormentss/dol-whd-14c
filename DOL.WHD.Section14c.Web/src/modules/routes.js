'use strict';
var config = require('./routes.config');

module.exports = function (app) {
  app.config(function($routeProvider) {
    $routeProvider
      .when('/', {
        controller: 'systemUseController',
        reloadOnSearch: false,
        template: require('./pages/systemUseTemplate.html'),
        access: config.access.ROUTE_PUBLIC,
        label: "14(c) Home"
      })
      .when('/dashboard', {
        controller: 'landingPageController',
        reloadOnSearch: false,
        template: require('./pages/landingPageTemplate.html'),
        access: config.access.ROUTE_LOGGEDIN,
        isLanding: true,
        label: 'Dashboard',
        parent:'/'
      })
      .when('/login', {
        controller: 'userLoginPageController',
        template: require('./pages/userLoginPageTemplate.html'),
        access: config.access.ROUTE_PUBLIC,
        reloadOnSearch: false,
        label: 'Login',
        parent: "/"
      })
      .when('/register', {
        controller: 'userRegistrationPageController',
        template: require('./pages/userRegistrationPageTemplate.html'),
        access: config.access.ROUTE_PUBLIC,
      })
      .when('/employerRegistration', {
        controller: 'employerRegistrationController',
        reloadOnSearch: false,
        template: require('./pages/employerRegistrationTemplate.html'),
        access: config.access.ROUTE_LOGGEDIN,
        label: "Employer Registration",
        parent: "/"
      })
      .when('/changePassword', {
        controller: 'changePasswordPageController',
        template: require('./pages/changePasswordPageTemplate.html'),
        access: config.access.ROUTE_PUBLIC,
        label: 'Change Password',
        parent: '/dashboard'
      })
      .when('/forgotPassword', {
        controller: 'forgotPasswordPageController',
        template: require('./pages/forgotPasswordPageTemplate.html'),
        access: config.access.ROUTE_PUBLIC,
        label: 'Forgot Password',
        parent: '/dashboard'
      })
      .when('/help', {
        controller: 'helpPageController',
        template: require('./pages/helpPage.html'),
        access: config.access.ROUTE_PUBLIC,
        label: 'Help',
        parent: '/dashboard'
      })
      .when('/account/:userId', {
        controller: 'accountPageController',
        template: require('./pages/accountPageTemplate.html'),
        access: config.access.ROUTE_LOGGEDIN,
        parent: '/dashboard',
        label: "Account"
      })
      .when('/section/assurances', {
        template: '<form-section><section-assurances></section-assurances></form-section>',
        reloadOnSearch: false,
        access: config.access.ROUTE_USER,
        label: 'Assurances',
        parent:'/'
      })
      .when('/section/app-info', {
        template: '<form-section><section-app-info></section-app-info></form-section>',
        reloadOnSearch: false,
        access: config.access.ROUTE_USER,
        label: 'Application Info',
        parent:'/'
      })
      .when('/section/employer', {
        template: '<form-section><section-employer></section-employer></form-section>',
        reloadOnSearch: false,
        access: config.access.ROUTE_USER,
        label: 'Employer',
        parent:'/'
      })
      .when('/section/wage-data', {
        template: '<form-section><section-wage-data></section-wage-data></form-section>',
        reloadOnSearch: false,
        access: config.access.ROUTE_USER,
        label: 'Wage Data',
        parent:'/'
      })
      .when('/section/work-sites', {
        template: '<form-section><section-work-sites></section-work-sites></form-section>',
        reloadOnSearch: false,
        access: config.access.ROUTE_USER,
        label: 'Work Sites & Employees',
        parent:'/'
      })
      .when('/section/wioa', {
        template: '<form-section><section-wioa></section-wioa></form-section>',
        reloadOnSearch: false,
        access: config.access.ROUTE_USER,
        label: 'WIOA',
        parent:'/'
      })
      .when('/section/review', {
        template: '<form-section><section-review></section-review></form-section>',
        reloadOnSearch: false,
        access: config.access.ROUTE_USER,
        label: 'Review',
        parent:'/'
      })
      .when('/admin', {
        controller: 'adminDashboardController',
        template: require('./pages/adminDashboardTemplate.html'),
        access: config.access.ROUTE_ADMIN,
        parent: '/dashboard',
        label: "Admin Dashboard"
      })
      .when('/admin/users', {
        controller: 'userManagementPageController',
        template: require('./pages/userManagementPageTemplate.html'),
        access: config.access.ROUTE_ADMIN
      })
      .when('/admin/:app_id', {
        redirectTo: function(params) {
          return '/admin/' + params.app_id + '/section/summary';
        },
        access: config.access.ROUTE_ADMIN
      })
      .when('/admin/:app_id/section/:section_id', {
        template: function(params) {
          return (
            '<admin-review appid=' +
            params.app_id +
            '><section-admin-' +
            params.section_id +
            ' item-id=' +
            (params.item_id || '') +
            '></section-admin-' +
            params.section_id +
            '></admin-review>'
          );
        },
        reloadOnSearch: false,
        access: config.access.ROUTE_ADMIN
      })
      .when('/v4/hello', { template: '<hello-world></hello-world>' })
      .when('/v4/ui-library', { template: '<ui-library></ui-library>' })
      .otherwise({
        redirectTo: '/'
      });
  });
}
