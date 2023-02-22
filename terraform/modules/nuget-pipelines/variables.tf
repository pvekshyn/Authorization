variable "azure_project_id" {
  type        = string
}

variable "github_serviceconnection_id" {
  type        = string
}

variable "azure_pipelines_folder" {
  type        = string
  default = "\\"
}

variable "nuget_name_yml_list" {
  type        = list(map(string))
}