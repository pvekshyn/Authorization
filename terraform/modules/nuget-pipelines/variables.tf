variable "azure_project_id" {
  type        = string
}

variable "github_serviceconnection_id" {
  type        = string
}

variable "pipelines_folder" {
  type        = string
  default     = "\\nugets"
}

variable "nuget_names" {
  type        = list(string)
  default     = [
    "Common.Domain",
    "Common.SDK",
    "Common.Application",
    "Common.Infrastructure",
    "Common.SpecFlowTests",
    "Inbox.SDK",
    "Outbox.SDK",
    "Role.SDK",
    "Assignment.SDK",
    "Authorization.SDK"
  ]
}