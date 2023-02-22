terraform {
  required_providers {
    azuredevops = {
      source = "microsoft/azuredevops"
      version = "=0.1.7"
    }
  }
}

provider "azuredevops" {
  org_service_url = "https://dev.azure.com/pvekshin/"
  personal_access_token = "zg7ednaie2xf42paf75reqiegznfezp5xjhksqsjmsdkb6ch7npq"
}

resource "azuredevops_project" "azure_project" {
  name               = var.azure_project_name
  visibility         = "private"
  version_control    = "Git"
  work_item_template = "Basic"
  features = {
      "boards" = "enabled"
      "repositories" = "enabled"
      "pipelines" = "enabled"
      "testplans" = "enabled"
      "artifacts" = "enabled"
  }
}

resource "azuredevops_serviceendpoint_github" "github_sc" {
  project_id            = azuredevops_project.azure_project.id
  service_endpoint_name = "GitHub Service Connection"

  auth_personal {
    personal_access_token = var.github_token
  }
}

module "nuget-pipelines" {
  source           = "./modules/nuget-pipelines"
  azure_project_id = azuredevops_project.azure_project.id
  github_serviceconnection_id = azuredevops_serviceendpoint_github.github_sc.id
  azure_pipelines_folder = "\\nugets"
  nuget_name_yml_list = [
    { name = "Common.Domain", path = "/nugets/Common.Domain.yml" },
    { name = "Common.SDK", path = "/nugets/Common.SDK.yml" },
    { name = "Common.Application", path = "/nugets/Common.Application.yml" },
    { name = "Common.Infrastructure", path = "/nugets/Common.Infrastructure.yml" },
    { name = "Common.SpecFlowTests", path = "/nugets/Common.SpecFlowTests.yml" },
    { name = "Inbox.SDK", path = "/nugets/Inbox.SDK.yml" },
    { name = "Outbox.SDK", path = "/nugets/Outbox.SDK.yml" },
    { name = "Role.SDK", path = "/nugets/Role.SDK.yml" },
    { name = "Assignment.SDK", path = "/nugets/Assignment.SDK.yml" },
    { name = "Authorization.SDK", path = "/nugets/Authorization.SDK.yml" },
  ]
}

module "service-pipelines" {
  source           = "./modules/nuget-pipelines"
  azure_project_id = azuredevops_project.azure_project.id
  github_serviceconnection_id = azuredevops_serviceendpoint_github.github_sc.id
  nuget_name_yml_list = [
    { name = "Role", path = "/Role/role-azure-pipelines.yml" },
    { name = "Assignment", path = "/Assignment/azure-pipelines.yml" },
    { name = "Authorization", path = "/Authorization/azure-pipelines.yml" },
    { name = "Role.Outbox.Job", path = "/Outbox.Job/role-outbox-azure-pipelines.yml" },
    { name = "Assignment.Outbox.Job", path = "/Outbox.Job/assignment-outbox-azure-pipelines.yml" },
    { name = "Assignment.Inbox.Job", path = "/Inbox.Job/assignment-inbox-azure-pipelines.yml" },
    { name = "Authorization.Inbox.Job", path = "/Inbox.Job/authorization-inbox-azure-pipelines.yml" }
  ]
}