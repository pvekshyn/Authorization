SqlPackage /Action:Publish /SourceFile:"Role\src\Role.Database\bin\Debug\Role.Database.dacpac" /Profile:"Role\src\Role.Database\local.publish.xml"
SqlPackage /Action:Publish /SourceFile:"Outbox.Job\src\Outbox.Database\bin\Debug\Outbox.Database.dacpac" /Profile:"Outbox.Job\src\Outbox.Database\role.publish.xml"

SqlPackage /Action:Publish /SourceFile:"Assignment\src\Assignment.Database\bin\Debug\Assignment.Database.dacpac" /Profile:"Assignment\src\Assignment.Database\local.publish.xml"
SqlPackage /Action:Publish /SourceFile:"Outbox.Job\src\Outbox.Database\bin\Debug\Outbox.Database.dacpac" /Profile:"Outbox.Job\src\Outbox.Database\assignment.publish.xml"
SqlPackage /Action:Publish /SourceFile:"Inbox.Job\src\Inbox.Database\bin\Debug\Inbox.Database.dacpac" /Profile:"Inbox.Job\src\Inbox.Database\assignment.publish.xml"

SqlPackage /Action:Publish /SourceFile:"Authorization\src\Authorization.Database\bin\Debug\Authorization.Database.dacpac" /Profile:"Authorization\src\Authorization.Database\local.publish.xml"
SqlPackage /Action:Publish /SourceFile:"Inbox.Job\src\Inbox.Database\bin\Debug\Inbox.Database.dacpac" /Profile:"Inbox.Job\src\Inbox.Database\authorization.publish.xml"

SqlPackage /Action:Publish /SourceFile:"Role\src\Role.Database\bin\Debug\Role.Database.dacpac" /Profile:"Role\src\Role.Database\integration.test.publish.xml"
SqlPackage /Action:Publish /SourceFile:"Outbox.Job\src\Outbox.Database\bin\Debug\Outbox.Database.dacpac" /Profile:"Outbox.Job\src\Outbox.Database\role.integration.test.publish.xml"

SqlPackage /Action:Publish /SourceFile:"Assignment\src\Assignment.Database\bin\Debug\Assignment.Database.dacpac" /Profile:"Assignment\src\Assignment.Database\integration.test.publish.xml"
SqlPackage /Action:Publish /SourceFile:"Outbox.Job\src\Outbox.Database\bin\Debug\Outbox.Database.dacpac" /Profile:"Outbox.Job\src\Outbox.Database\assignment.integration.test.publish.xml"

pause