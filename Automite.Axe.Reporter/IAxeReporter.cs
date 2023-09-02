using Deque.AxeCore.Commons;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;

namespace Automite.Axe.Reporter
{
    public interface IAxeReporter
    {
        void Generate(AxeResult result);
        void GenerateHtml(AxeResult result, string? fileName = null);
    }

    public class AxeReporterImpl : IAxeReporter
    {
        public void GenerateHtml(AxeResult result, string? fileName = null)
        {
            string defaultFileName = "Accessibility Test Report";
            string htmlTemplate = GetHtmlTemplate();
            string fileTitle = fileName ?? defaultFileName;

            htmlTemplate = htmlTemplate.Replace("{title}", fileTitle);
            htmlTemplate = SetEnvironmentDetail(htmlTemplate, result);
            htmlTemplate = SetViolations(htmlTemplate, result);
            htmlTemplate = SetPasses(htmlTemplate, result);
            htmlTemplate = SetIncomplete(htmlTemplate, result);
            htmlTemplate = SetInapplicable(htmlTemplate, result);

            // Write the HTML content to a file
            File.WriteAllText($"{fileTitle}.html", htmlTemplate);

            Console.WriteLine("HTML report generated successfully!");
        }

        private string SetInapplicable(string htmlTemplate, AxeResult result)
        {
            int index = 0;
            string tableRows = "";
            foreach (var inapplicableResult in result.Inapplicable)
            {
                index += 1;
                string impact = inapplicableResult.Impact;
                string description = inapplicableResult.Description.Replace("<", "&lt;").Replace(">", "&gt;");
                string help = inapplicableResult.Help.Replace("<", "&lt;").Replace(">", "&gt;");
                string helpUrl = $"Reference <a href=\"{inapplicableResult.HelpUrl}\" target=\"_blank\" rel=\"noopener noreferrer\">{inapplicableResult.HelpUrl}</a>";
                string tags = string.Join(", ", inapplicableResult.Tags);
                string id = inapplicableResult.Id;
                string nodes = JsonConvert.SerializeObject(inapplicableResult.Nodes, Formatting.Indented).Replace("<", "&lt;").Replace(">", "&gt;");

                tableRows +=
                    $@"<tr class=""table-sm text-left"">
                        <th scope=""row"" class=""bg-dark table-dark"">{index}</th>
                        <td>{impact}</td>                        
                        <td>{id}</td>
                        <td>{description}</td>
                        <td>{help} </br> {helpUrl}</td>
                        <td>{tags}</td>
                        <td>
                            <button type=""button"" class=""btn btn-link"" data-toggle=""collapse"" data-target=""#inapplicable-detail-{index}"">Details</button>
                            <div id=""inapplicable-detail-{index}"" class=""collapse"">
                                <pre><code>{nodes}</code></pre></td>
                            </div>
                    </tr>";
            }
            return htmlTemplate.Replace("{inapplicable}", tableRows);
        }

        private string SetIncomplete(string htmlTemplate, AxeResult result)
        {
            int index = 0;
            string tableRows = "";
            foreach (var incompleteResult in result.Incomplete)
            {
                index += 1;
                string impact = incompleteResult.Impact;
                string description = incompleteResult.Description.Replace("<", "&lt;").Replace(">", "&gt;");
                string help = incompleteResult.Help.Replace("<", "&lt;").Replace(">", "&gt;");
                string helpUrl = $"Reference <a href=\"{incompleteResult.HelpUrl}\" target=\"_blank\" rel=\"noopener noreferrer\">{incompleteResult.HelpUrl}</a>";
                string tags = string.Join(", ", incompleteResult.Tags);
                string id = incompleteResult.Id;
                string nodes = JsonConvert.SerializeObject(incompleteResult.Nodes, Formatting.Indented).Replace("<", "&lt;").Replace(">", "&gt;");

                tableRows +=
                    $@"<tr class=""table-sm text-left"">
                        <th scope=""row"" class=""bg-warning"">{index}</th>
                        <td>{impact}</td>                        
                        <td>{id}</td>
                        <td>{description}</td>
                        <td>{help} </br> {helpUrl}</td>
                        <td>{tags}</td>
                        <td>
                            <button type=""button"" class=""btn btn-link"" data-toggle=""collapse"" data-target=""#incomplete-detail-{index}"">Details</button>
                            <div id=""incomplete-detail-{index}"" class=""collapse"">
                                <pre><code>{nodes}</code></pre></td>
                            </div>
                    </tr>";
            }
            return htmlTemplate.Replace("{incomplete}", tableRows);
        }

        private string SetPasses(string htmlTemplate, AxeResult result)
        {
            int index = 0;
            string tableRows = "";
            foreach (var passResult in result.Passes)
            {
                index += 1;
                string impact = passResult.Impact;
                string description = passResult.Description.Replace("<", "&lt;").Replace(">", "&gt;");
                string help = passResult.Help.Replace("<", "&lt;").Replace(">", "&gt;");
                string helpUrl = $"Reference <a href=\"{passResult.HelpUrl}\" target=\"_blank\" rel=\"noopener noreferrer\">{passResult.HelpUrl}</a>";
                string tags = string.Join(", ", passResult.Tags);
                string id = passResult.Id;
                string nodes = JsonConvert.SerializeObject(passResult.Nodes, Formatting.Indented).Replace("<", "&lt;").Replace(">", "&gt;");

                tableRows +=
                    $@"<tr class=""table-sm text-left"">
                        <th scope=""row"" class=""bg-success"">{index}</th>
                        <td>{impact}</td>                        
                        <td>{id}</td>
                        <td>{description}</td>
                        <td>{help} </br> {helpUrl}</td>
                        <td>{tags}</td>
                        <td>
                            <button type=""button"" class=""btn btn-link"" data-toggle=""collapse"" data-target=""#passes-detail-{index}"">Details</button>
                            <div id=""passes-detail-{index}"" class=""collapse"">
                                <pre><code>{nodes}</code></pre></td>
                            </div>
                    </tr>";
            }
            return htmlTemplate.Replace("{passes}", tableRows);
        }

        private string SetViolations(string htmlTemplate, AxeResult result)
        {
            int index = 0;
            string tableRows = "";
            foreach (var violation in result.Violations)
            {
                index += 1;
                string impact = violation.Impact;
                string description = violation.Description.Replace("<", "&lt;").Replace(">", "&gt;");
                string help = violation.Help.Replace("<", "&lt;").Replace(">", "&gt;");
                string helpUrl = $"Reference <a href=\"{violation.HelpUrl}\" target=\"_blank\" rel=\"noopener noreferrer\">{violation.HelpUrl}</a>";
                string tags = string.Join(", ", violation.Tags);
                string id = violation.Id;
                string nodes = JsonConvert.SerializeObject(violation.Nodes, Formatting.Indented).Replace("<", "&lt;").Replace(">", "&gt;");

                tableRows +=
                    $@"<tr class=""table-sm text-left"">
                        <th scope=""row"" class=""{GetMachingColor(impact)}"">{index}</th>
                        <td>{impact}</td>                        
                        <td>{id}</td>
                        <td>{description}</td>
                        <td>{help} </br> {helpUrl}</td>
                        <td>{tags}</td>
                        <td>
                            <button type=""button"" class=""btn btn-link"" data-toggle=""collapse"" data-target=""#violation-detail-{index}"">Details</button>
                            <div id=""violation-detail-{index}"" class=""collapse"">
                                <pre><code>{nodes}</code></pre></td>
                            </div>
                    </tr>";
            }
            return htmlTemplate.Replace("{violations}", tableRows);
        }

        private string SetEnvironmentDetail(string htmlTemplate, AxeResult result)
        {
            htmlTemplate = htmlTemplate.Replace("{test-engine}", result.TestEngine.Name ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{test-engine-version}", result.TestEngine.Version ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{orientation-angle}", result.TestEnvironment.OrientationAngle.ToString() ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{orientation-type}", result.TestEnvironment.OrientationType ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{user-agent}", result.TestEnvironment.UserAgent ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{window-width}", result.TestEnvironment.WindowWidth.ToString() ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{window-height}", result.TestEnvironment.WindowHeight.ToString() ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{timestamp}", result.Timestamp.ToString() ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{test-url}", result.Url ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{test-runner}", result.TestRunner.Name ?? "Unavailable");
            htmlTemplate = htmlTemplate.Replace("{tool-option}", JsonConvert.SerializeObject(result.ToolOptions));
            return htmlTemplate;
        }

        public void Generate(AxeResult result)
        {
            var violations = result.Violations;
            foreach (var violation in violations)
            {
                OutputViolation(violation);
            }
        }

        private void OutputViolation(AxeResultItem item)
        {
            Console.WriteLine("==========================");
            Console.WriteLine("Impact : " + item.Impact);
            Console.WriteLine("Description : " + item.Description);
            Console.WriteLine("Help : " + item.Help);
            Console.WriteLine("HelpUrl : " + item.HelpUrl);
            Console.WriteLine("Tags : " + string.Join(", ", item.Tags));
            Console.WriteLine("Impacted elements: " + JsonConvert.SerializeObject(item.Nodes));

        }
        private string GetMachingColor(string impact)
        {
            switch (impact)
            {
                case "serious": return "bg-danger";
                case "moderate": return "bg-warning";
                case "minor": return "bg-info";
                default: return "bg-default";
            }
        }
        private string GetHtmlTemplate()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Automite.Axe.Reporter.assets.report.html";
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
                throw new Exception($"Unable to find resource {resourceName} in assembly {assembly.FullName}");
            return new StreamReader(stream).ReadToEnd();
        }
    }
}
