'use strict';

module.exports = function(ngModule) {
  ngModule.controller('employerRegistrationController', function(
    $scope,
    stateService,
    $location,
    apiService
  ) {
    'ngInject';
    'use strict';
    $scope.showAllHelp = false
    $scope.stateService = stateService;

    $scope.navToLanding = function() {
      $location.path('/dashboard');
    };

    $scope.showDetails = false;
    $scope.toggleDetails = function ()  {
      $scope.showDetails = !$scope.showDetails;
      log.info($scope.showDetails);
    };

    $scope.toggleAllHelpText = function () {
      $scope.showAllHelp = !$scope.showAllHelp;
    };

    $scope.onSubmitClick = function () {
      $scope.formData.IsPointOfContact = true;
      $scope.formData.employer.ein = $scope.formData.ein;
      apiService.setEmployer($scope.stateService.access_token, $scope.formData).then(function() {
          $scope.registrationSuccess = true;
      }).catch(function(error) {
         if(error.status === 302) {
            $scope.previouslyRegistered = {};
            $scope.previouslyRegistered.status = true;
            $scope.previouslyRegistered.name = error.data;
          }
      });
    }
  });
};