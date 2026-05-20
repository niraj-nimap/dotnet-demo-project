pipeline {
    agent { label 'windows' }

    environment {
        APP_NAME = "MyDotnetApp"
        PUBLISH_DIR = "publish"
        IIS_PATH = "C:\\inetpub\\wwwroot\\MyDotnetApp"
    }

    stages {

        stage('Install Dependencies') {
            steps {
                bat '''
                echo Installing .NET SDK...
                winget install --id Microsoft.DotNet.SDK.8 -e --silent

                echo Installing Git...
                winget install --id Git.Git -e --silent

                echo Installing Node.js...
                winget install --id OpenJS.NodeJS.LTS -e --silent

                echo Installing VC++ Runtime...
                winget install --id Microsoft.VCRedist.2015+.x64 -e --silent

                echo Enabling IIS...
                powershell -Command "Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole -All -NoRestart"

                powershell -Command "Enable-WindowsOptionalFeature -Online -FeatureName IIS-ASPNET45 -All -NoRestart"

                powershell -Command "Enable-WindowsOptionalFeature -Online -FeatureName IIS-ManagementConsole -All -NoRestart"
                '''
            }
        }

        stage('Verify Tools') {
            steps {
                bat '''
                dotnet --version
                git --version
                node -v
                npm -v
                '''
            }
        }

        stage('Checkout Code') {
            steps {
                git 'https://github.com/niraj-nimap/dotnet-demo-project'
            }
        }

        stage('Restore Packages') {
            steps {
                bat 'dotnet restore'
            }
        }

        stage('Build Application') {
            steps {
                bat 'dotnet build --configuration Release'
            }
        }

        stage('Publish Application') {
            steps {
                bat 'dotnet publish -c Release -o publish'
            }
        }

        stage('Create IIS Folder') {
            steps {
                bat '''
                if not exist "%IIS_PATH%" (
                    mkdir "%IIS_PATH%"
                )
                '''
            }
        }

        stage('Stop IIS Site') {
            steps {
                bat '''
                powershell -Command "Import-Module WebAdministration"
                powershell -Command "Stop-Website -Name Default Web Site"
                '''
            }
        }

        stage('Deploy Application') {
            steps {
                bat '''
                xcopy /E /Y /I publish\\* "%IIS_PATH%"
                '''
            }
        }

        stage('Start IIS Site') {
            steps {
                bat '''
                powershell -Command "Import-Module WebAdministration"
                powershell -Command "Start-Website -Name Default Web Site"
                '''
            }
        }

        stage('Deployment Success') {
            steps {
                bat 'echo Deployment Completed Successfully'
            }
        }
    }
}