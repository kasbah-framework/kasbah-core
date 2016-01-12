$OpenCover = "C:\\apps\\opencover.4.6.210\\OpenCover.Console.exe"
$ReportGenerator = "C:\\apps\\ReportGenerator_2.3.3.0\\bin\\ReportGenerator.exe"

$DNX = "C:\\Users\\Brendan\\.dnx\\runtimes\\dnx-clr-win-x86.1.0.0-rc1-update1\\bin\\dnx.exe"
$ProjectRoot = "C:\\dev\\personal\\kasbah\\kasbah-core\\src\\"
$TestRoot = "C:\\dev\\personal\\kasbah\\kasbah-core\\test\\"

[string[]] $Projects = "Kasbah.Core", "Kasbah.Core.ContentTree.Npgsql", "Kasbah.Core.Events", "Kasbah.Core.Index", "Kasbah.Core.Index.Solr", "Kasbah.Core.Events.Redis", "Kasbah.Core.ContentTree", "Kasbah.Core.ContentBroker", ""

Remove-Item -Force -Confirm:$false -Recurse coverage
New-Item -Type Directory -Force coverage

foreach ($Project in $Projects) {
    if ($Project -eq "") { continue; }

    dnu build --quiet ("$ProjectRoot" + "$Project" + "\\project.json")

    $ProjectPath = $ProjectRoot + $Project + "\\bin\\debug\\dnx451"
    $TestPath = $TestRoot + $Project + ".Tests"

    &$OpenCover -target:"$DNX" -targetargs:"--lib $ProjectPath -p $TestPath test" -output:"coverage\\$Project.coverage.xml" -filter:+[$Project]* -register
}

&$ReportGenerator -reports:"coverage\\*.coverage.xml" -targetdir:"coverage"
