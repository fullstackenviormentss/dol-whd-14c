'use strict';

module.exports = function(ngModule) {
  require('./accountPageController')(ngModule);
  require('./adminDashboardController')(ngModule);
  require('./appReviewPageController')(ngModule);
  require('./changePasswordPageController')(ngModule);
  require('./employerRegistrationController')(ngModule);
  require('./landingPageController')(ngModule);
  require('./systemUseController')(ngModule);
  require('./userLoginPageController')(ngModule);
  require('./userManagementPageController')(ngModule);
  require('./userRegistrationPageController')(ngModule);
  require('./forgotPasswordPageController')(ngModule);
  require('./helpPageController')(ngModule);
  require('./dashboardTableConfig');
};
