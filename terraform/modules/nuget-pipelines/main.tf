terraform {
  required_providers {
    azuredevops = {
      source = "microsoft/azuredevops"
      version = "=0.1.7"
    }
  }
}

resource "azuredevops_build_definition" "nuget_build" {
  for_each = toset(var.nuget_names)

  project_id = var.azure_project_id
  name       = each.value
  path       = var.pipelines_folder

  ci_trigger {
    use_yaml = true
  }

  repository {
    repo_type             = "GitHub"
    repo_id               = "pvekshyn/Authorization"
    branch_name           = "main"
    yml_path              = "${var.pipelines_folder}/${each.value}.yml"
    service_connection_id = var.github_serviceconnection_id
  }
}
