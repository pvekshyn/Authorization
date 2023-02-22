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

resource "azuredevops_project" "terraform_project" {
  name               = "Terraform"
  description        = "Terraform test project"
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
  project_id            = azuredevops_project.terraform_project.id
  service_endpoint_name = "GitHub Service Connection"

  auth_personal {
    personal_access_token = "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"
  }
}

module "nuget-pipelines" {
  source           = "./modules/nuget-pipelines"
  azure_project_id = azuredevops_project.terraform_project.id
  github_serviceconnection_id = azuredevops_serviceendpoint_github.github_sc.id
}