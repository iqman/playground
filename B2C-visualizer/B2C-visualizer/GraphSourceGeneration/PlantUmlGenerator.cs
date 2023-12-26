﻿using B2C_visualizer.Model;

namespace B2C_visualizer.GraphSourceGeneration
{
    class PlantUmlGenerator : IGraphGenerator
    {
        public string Generate(IEnumerable<ServicePrincipal> sps)
        {
            var header =
@"@startuml

skin rose
skinparam componentStyle rectangle
left to right direction
skinparam nodesep 10
skinparam ranksep 150
title Components - Component Diagram";

            var footer =
@"
@enduml
";

            var strings = new List<string>();
            strings.Add(header);
            strings.AddRange(sps.Select(GenerateSubGraph));
            strings.AddRange(sps.Select(GenerateRelations));
            strings.Add(footer);

            string plantuml = string.Join(Environment.NewLine, strings);

            return plantuml;
        }

        private string GenerateRelations(ServicePrincipal sp)
        {
            var relations = sp.GrantedResourceAccesses.Any()
                ? string.Join(Environment.NewLine, sp.GrantedResourceAccesses.SelectMany(res => res.Roles).Select(r => string.Format($"{sp.AppId.ShortenId()} --> {r.Id.ShortenId()}")))
                : string.Empty;

            return relations;
        }

        private string GenerateSubGraph(ServicePrincipal sp)
        {
            string appRoles = GenerateAppRoleNodes(sp);

            string oauth2Permissions = GenerateOauth2RoleNodes(sp);


            // this throws an exception at runtime for unknown reasons... so doing it the hard way below instead... sigh
            //            var subgraph = string.Format($$"""
            //package "{{sp.Name}}" as {{sp.AppId.ShortenId()}} {


            //    package "App Roles" as {{sp.AppId.ShortenId()}}_app_roles {
            //{{appRoles}}
            //    }
            //    package "Scopes" as {{sp.AppId.ShortenId()}}_scopes {
            //{{oauth2Permissions}}
            //    }
            //}
            //""");


            
            var a = string.Format($"component \"{sp.Name}\" as {sp.AppId.ShortenId()}") + " {" + Environment.NewLine;
            var b = string.Format($"component \"App Roles\" as {sp.AppId.ShortenId()}_app_roles") + " {" + Environment.NewLine;
            var c = appRoles + Environment.NewLine;
            var d = "}" + Environment.NewLine;
            var e = string.Format($"component \"Scopes\" as {sp.AppId.ShortenId()}_scopes") + " {" + Environment.NewLine;
            var f = oauth2Permissions + Environment.NewLine;
            var g = "}" + Environment.NewLine;
            var h = "}";

            var subgraph = a + b + c + d + e + f + g + h;


            // conside to add the plantuml equivalent of these mermaid lines
            //{sp.AppId.ShortenId()}_link["<u>B2C</u>"]
            //style {sp.AppId.ShortenId()}_link fill:#fff0,stroke:#fff0
            // click {sp.AppId.ShortenId()}_link href "https://portal.azure.com/#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Overview/appId/{sp.AppId}/isMSAApp~/false"
            return subgraph;
        }

        private static string GenerateOauth2RoleNodes(ServicePrincipal sp)
        {
            return sp.DefinedOauth2Permissions.Any()
                ? string.Join(Environment.NewLine, sp.DefinedOauth2Permissions.Select(r => string.Format($"    [{r.Value}] as {r.Id.ShortenId()}")))
                : string.Format($"    [None] as {sp.AppId.ShortenId()}_oauth2_perm_none");
        }

        private static string GenerateAppRoleNodes(ServicePrincipal sp)
        {
            return sp.DefinedAppRoles.Any()
                ? string.Join(Environment.NewLine, sp.DefinedAppRoles.Select(r => string.Format($"    [{r.Value}] as {r.Id.ShortenId()}")))
                : string.Format($"    [None] as {sp.AppId.ShortenId()}_app_roles_none");
        }
    }
}
