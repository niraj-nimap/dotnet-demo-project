pipeline {
    agent { label 'windows' }

    environment {
        APP_NAME    = "MyDotnetApp"
        PROJECT_DIR = "project"
        PUBLISH_DIR = "publish"
        IIS_PATH    = "C:\\inetpub\\wwwroot\\MyDotnetApp"
        GIT_PATH    = "C:\\Program Files\\Git\\cmd\\git.exe"
    }

    stages {

        stage('Install Dependencies') {
            steps {
                bat '''
                echo =====================================
                echo Installing Dependencies
                echo =====================================

                winget install --id Microsoft.DotNet.SDK.8 -e --silent
                winget install --id Git.Git -e --silent
                winget install --id OpenJS.NodeJS.LTS -e --silent
                winget install --id Microsoft.VCRedist.2015+.x64 -e --silent

                powershell -Command "Enable-WindowsOptionalFeature -Online -FeatureName IIS-WebServerRole -All -NoRestart"

                powershell -Command "Enable-WindowsOptionalFeature -Online -FeatureName IIS-ASPNET45 -All -NoRestart"

                powershell -Command "Enable-WindowsOptionalFeature -Online -FeatureName IIS-ManagementConsole -All -NoRestart"
                '''
            }
        }

        stage('Verify Tools') {
            steps {
                bat '''
                echo =====================================
                echo Verifying Installed Tools
                echo =====================================

                dotnet --version

                "%GIT_PATH%" --version

                node -v

                npm -v
                '''
            }
        }

        stage('Clean Workspace') {
            steps {
                bat '''
                if exist "%PROJECT_DIR%" (
                    rmdir /s /q "%PROJECT_DIR%"
                )

                if exist "%PUBLISH_DIR%" (
                    rmdir /s /q "%PUBLISH_DIR%"
                )
                '''
            }
        }

        stage('Clone Repository') {
            steps {
                bat '''
                echo =====================================
                echo Cloning Repository
                echo =====================================

                "%GIT_PATH%" clone https://github.com/niraj-nimap/dotnet-demo-project.git "%PROJECT_DIR%"
                '''
            }
        }

        stage('Restore Packages') {
            steps {
                dir("${PROJECT_DIR}") {
                    bat 'dotnet restore'
                }
            }
        }

        stage('Build Application') {
            steps {
                dir("${PROJECT_DIR}") {
                    bat 'dotnet build --configuration Release'
                }
            }
        }

        stage('Publish Application') {
            steps {
                dir("${PROJECT_DIR}") {
                    bat 'dotnet publish -c Release -o ..\\publish'
                }
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

        stage('Stop IIS Website') {
            steps {
                bat '''
                powershell -Command "Import-Module WebAdministration"

                powershell -Command "Stop-Website -Name \\"Default Web Site\\""
                '''
            }
        }

        stage('Deploy Application') {
            steps {
                bat '''
                echo =====================================
                echo Deploying Application
                echo =====================================

                xcopy /E /H /C /I /Y "%PUBLISH_DIR%\\*" "%IIS_PATH%"
                '''
            }
        }

        stage('Start IIS Website') {
            steps {
                bat '''
                powershell -Command "Import-Module WebAdministration"

                powershell -Command "Start-Website -Name \\"Default Web Site\\""
                '''
            }
        }

        stage('Deployment Success') {
            steps {
                bat '''
                echo =====================================
                echo Deployment Completed Successfully
                echo =====================================
                '''
            }
        }
    }

    post {
        success {
            echo 'Application deployed successfully.'
        }

        failure {
            echo 'Pipeline failed.'
        }
    }
}