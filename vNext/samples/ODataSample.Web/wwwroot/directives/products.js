"use strict";
angular
	.module("odataSampleApp")
	.directive("products", function () {
		return {
			restrict: "E",
			scope: {

			},
			templateUrl: "/directives/products.html",
			controller: [
				"$http", "$scope",
				function ($http, $scope) {
					$scope.$on("resolve-field-validation-url", function (e, config) {
						config.setName = "Products";
						config.fieldValidationUrl = "/odata/ValidateField";
					});
					$scope.addProduct = function () {
						$http.post("odata/Products", {
								Name: $scope.Name,
								Price: $scope.Price
							})
							.then(function(success) {
									$scope.odataErrors = null;
								},
								function(errors) {
									$scope.odataErrors = errors;
								});
					};
				}
			]
		};
	});