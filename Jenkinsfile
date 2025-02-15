pipeline {
    agent any

    environment {
        DOCKER_IMAGE_PROD = "learnjenkinsapi:latest"
        DOCKER_IMAGE_DEV = "learnjenkinsapi-dev:latest"
        BRANCH_NAME = "${env.GIT_BRANCH}" // Ambil nama branch yang dipush
    }

    stages {
        stage('Checkout Code') {
            steps {
                git 'https://fadhlihgp@github.com/fadhlihgp/LearnJenkins.git
'
            }
        }

        stage('Determine Deployment Target') {
            steps {
                script {
                    if (BRANCH_NAME == 'origin/dev') {
                        env.TARGET_ENV = 'Development'
                        env.CONTAINER_NAME = 'learnjenkinsapi_dev'
                        env.PORT = '7003'
                        env.DOCKER_IMAGE = env.DOCKER_IMAGE_DEV
                    } else if (BRANCH_NAME == 'origin/main' || BRANCH_NAME == 'origin/master') {
                        env.TARGET_ENV = 'Production'
                        env.CONTAINER_NAME = 'learnjenkinsapi_prod'
                        env.PORT = '7002'
                        env.DOCKER_IMAGE = env.DOCKER_IMAGE_PROD
                    } else {
                        error "Branch tidak dikenali. Deployment hanya untuk 'dev' atau 'main/master'."
                    }
                }
            }
        }

        stage('Build Docker Image') {
            steps {
                sh "docker build -t $DOCKER_IMAGE -f Dockerfile ."
            }
        }

        stage('Deploy Application') {
            steps {
                script {
                    if (env.TARGET_ENV == 'Development') {
                        sh 'docker-compose -f docker-compose.override.yml up -d --build'
                    } else if (env.TARGET_ENV == 'Production') {
                        sh 'docker-compose -f docker-compose.yml up -d --build'
                    }
                }
            }
        }
    }
}
