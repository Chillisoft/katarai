require "./rake-tasks/init"

#-----------------------------------SETTINGS-----------------------------------

$binaries_baselocation = "bin"
$binaries_location = "#{$binaries_baselocation}/release"
$nunittesting_location = "#{$binaries_baselocation}/nunittesting"

#--------------------------------build settings--------------------------------

$build_configuration = "debug"
$app_version = "0.7.15"            #Major version
$release_type = "Debug"         #Debug, Staging or Production

def msbuild_settings
    {
      :properties => {:configuration => $build_configuration},
      :targets => [:clean, :rebuild],
      :verbosity => :minimal,
      :parameters => [ "/m", "/p:TargetProfile=Local" ],
    }
end

#------------------------------dependency settings-----------------------------
#-------------------------------project settings-------------------------------

$solution = "source/Katarai.sln"

#______________________________________________________________________________
#-------------------------------------TASKS------------------------------------

desc "Builds Katarai"
task :default => [:build_test]

desc "Builds and tests Katarai without coverage"
task :build_test => [:build_only, :test]

desc "Builds and tests Katarai with coverage"
task :build_coverage => [:build_only, :test_with_coverage]

desc "Only builds"
task :build_only => [:update_git_submodules, :clean, :update_packages, :msbuild, :copy_to_bin]

desc "Build with installer"
task :build_with_installer => [:update_git_submodules, :clean, :update_packages, :msbuild, :copy_to_bin, :compile_setup]

desc "Builds solution with Sonar analysis"
task :build_with_sonar => [:build_coverage, :sonar]

#---------------------------------Update stuff---------------------------------

desc "Updates packages with nuget"
updatenugetpackages :update_packages do |nuget|
    puts cyan("Updating nuget packages")
    nuget.solution = $solution
end

#------------------------Prepare For Katarai Build------------------------

desc "Cleans the bin folder and all project bin folders"
task :clean do
    puts cyan("Cleaning all bin and test folders")
    FileUtils.rm_rf $binaries_baselocation
    clean_build_output_directories
    clean_coverage_files
end

def clean_build_output_directories
    bin_dirs = Dir.glob("source/**/bin").select {|f| (File.directory? f) && !(f.match(/\/packages\//) || f.match(/\/obj\//))}
    for project_output in bin_dirs
        FileUtils.rm_rf project_output
    end
end

def clean_coverage_files
    FileUtils.rm_rf("buildreports/coverage")
end

task :get_test_projects do
    all_dirs = Dir.glob("**/*").select {|f| File.directory? f}
    $test_projects = all_dirs.select {|t| (t.match(/[.]Tests\z/) || t.match(/[.]KataData[.]/) || t.match(/SamplePlayer.*Kata\z/)) && (Dir.exists?(File.join(t, "bin", $build_configuration)))}
    $test_dlls = $test_projects.collect {|s| s.split("/").last}
end

#--------------------------------Build Solution--------------------------------

desc "Builds the Katarai solution"
msbuild :msbuild do |msb|
    puts cyan("Building #{$solution} with msbuild")
    msb.update_attributes msbuild_settings
    msb.solution = $solution
end
#--------------------------------Build Installer-------------------------------

innosetup :compile_setup do |setup|
    if $release_type == "Staging"
        MyAppId = "{83ED890F-69C1-4DBF-9913-A7B2EAA6C21D}"
    else
        MyAppId = "{CD2DF16B-1FEB-468C-AFA1-BB8910401D13}"
    end

    setup.ScriptFile = "katarai.iss"
    setup.Defines = {
        "MyAppId" => MyAppId,
        "MyAppVersion" => $app_version,
        #"ReleaseType" => $release_type,
    }
end
#----------------------------------Copy Tasks----------------------------------

task :copy_to_bin => [:get_test_projects] do
    puts cyan("Copying built files to the bin folder")
    FileSystem.EnsurePath($binaries_baselocation)
    FileSystem.EnsurePath($binaries_location)
    FileSystem.EnsurePath($nunittesting_location)
    copy_application_files_to $binaries_location
    copy_application_files_to $nunittesting_location
    copy_release_files_to $nunittesting_location
    copy_nunittesting_files_to $nunittesting_location
end


def copy_application_files_to location
    copy_release_files_to location
end

def copy_release_files_to location
    FileUtils.cp_r Dir.glob("source/Katarai.Runner/bin/#{$build_configuration}/Newtonsoft.Json.dll"), location
    FileUtils.cp_r Dir.glob("source/Katarai.Wpf/bin/#{$build_configuration}/*.dll"), location
    FileUtils.cp_r Dir.glob("source/Katarai.Wpf/bin/#{$build_configuration}/*.pdb"), location
    FileUtils.cp_r Dir.glob("source/Katarai.Wpf/bin/#{$build_configuration}/*.config"), location
    FileUtils.cp_r Dir.glob("source/Katarai.Wpf/bin/#{$build_configuration}/Katarai.exe"), location
    FileUtils.cp_r Dir.glob("source/Katarai.Wpf/bin/#{$build_configuration}/splash_screen.png"), location
end

def copy_test_project_output_to(project, location)
    if Dir.exists?(File.join(project, "bin", $build_configuration))
        FileUtils.cp_r File.join(project, "bin", $build_configuration, "/."), location
    else
        FileUtils.cp_r File.join(project, "bin", "/."), location
    end
end

def copy_nunittesting_files_to location
    for project in $test_projects
        copy_test_project_output_to project, location
    end
end


#-----------------------------------Run Tests----------------------------------

def testassemblies
    $test_dlls.map {|a| File.join($nunittesting_location, a + ".dll")}
end

desc "Runs the tests"
nunit :test => [:get_test_projects] do |nunit|
    puts cyan("Running tests")
    nunit.assemblies testassemblies
    nunit.parameters = [ "--process=Separate" ]
end

desc "Runs the tests with dotcover"
dotcover :test_with_coverage => [:get_test_projects] do |dc|
    puts cyan("Running tests with dotcover")
    dc.assemblies = testassemblies
    dc.nunitoptions = [ "--process=Separate" ]
    dc.filters = "+:module=*;class=*;function=*;-:*.Tests"
end





