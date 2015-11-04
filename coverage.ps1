$OpenCover = "C:\\apps\\opencover.4.6.210\\OpenCover.Console.exe"
$ReportGenerator = "C:\\apps\\ReportGenerator_2.3.3.0\\bin\\ReportGenerator.exe"

$DNX = "C:\\Users\\Brendan\\.dnx\\runtimes\\dnx-clr-win-x86.1.0.0-beta8\\bin\\dnx.exe"
$ProjectRoot = "C:\\dev\\personal\\kasbah-core\\src\\"
$TestRoot = "C:\\dev\\personal\\kasbah-core\\test\\"

[string[]] $Projects = "Kasbah.Core", "Kasbah.Core.ContentTree", "Kasbah.Core.ContentTree.Npgsql", "Kasbah.Core.Events"

New-Item -Type Directory -Force coverage

foreach ($Project in $Projects) {
    $ProjectPath = $ProjectRoot + "\\" + $Project + "\\bin\\debug\\dnx451"
    $TestPath = $TestRoot + "\\" + $Project + ".Tests"

    &$OpenCover -target:"$DNX" -targetargs:"--lib $ProjectPath -p $TestPath test" -output:"coverage\\$Project.coverage.xml" -filter:+[$Project]* -register
}

&$ReportGenerator -reports:"coverage\\*.coverage.xml" -targetdir:"coverage"
