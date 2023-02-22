terraform {
  required_providers {
    azuredevops = {
      source = "microsoft/azuredevops"
      version = "=0.1.7"
    }
  }
}

resource "azuredevops_build_definition" "nuget_build" {
  for_each = { for obj in var.nuget_name_yml_list : obj.name => obj }

  project_id = var.azure_project_id
  name       = each.value.name
  path       = var.azure_pipelines_folder

  ci_trigger {
    use_yaml = true
  }

  repository {
    repo_type             = "GitHub"
    repo_id               = "pvekshyn/Authorization"
    branch_name           = "main"
    yml_path              = each.value.path
    service_connection_id = var.github_serviceconnection_id
  }
}
