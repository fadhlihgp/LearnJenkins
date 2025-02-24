pipeline {
    agent any

    environment {
        DOCKER_IMAGE = "learn-jenkins-dev"
        VERSION = "latest"
        HOST_PORT = "7002"
        CONTAINER_PORT = "8080"
        HOST_LOG_DIR = "/var/www/Apps/LearnJenkinsDev"
        CONTAINER_LOG_DIR = "/var/www/Apps/LearnJenkinsDev"
    }

    stages {
        stage('Checkout') {
            steps {
                  git branch: '**', credentialsId: 'github-credentials', url: 'https://github.com/fadhlihgp/LearnJenkins.git'
              }
        }

        stage('Set Environment') {
            steps {
                script {
                    if (env.GIT_BRANCH == "origin/main" || env.GIT_BRANCH == "origin/master" || env.GIT_BRANCH == "main" || env.GIT_BRANCH == "master") {
                        HOST_PORT = "7001"
                        DOCKER_IMAGE = "learn-jenkins-prod"
                        HOST_LOG_DIR = "/var/www/Apps/LearnJenkinsProd"
                        CONTAINER_LOG_DIR = "/var/www/Apps/LearnJenkinsProd"
                    } else if (env.GIT_BRANCH == "dev" || env.GIT_BRANCH == "origin/dev") {
                        HOST_PORT = "7002"
                    }
                    echo "Deploying branch ${env.GIT_BRANCH} to port ${HOST_PORT}"
                }
            }
        }

        stage('Remove Old Container and Image ') {
            steps {
                script {
                    def containerId = sh(script: "docker ps -q --filter ancestor=${DOCKER_IMAGE}:${VERSION}", returnStdout: true).trim()
                    if(containerId) {
                        echo "Stop and remove old container: ${containerId}"
                        sh "docker stop ${containerId}"
                        sh "docker rm ${containerId}"
                    } else {
                        echo "No container is running."
                    }
                    def imageId = sh(script: "docker images -q ${DOCKER_IMAGE}:${VERSION}", returnStdout: true).trim()
                    if(imageId) {
                        echo "Remove old image: ${imageId}"
                        sh "docker rmi -f ${imageId}"
                    }
                }
            }
        }
        
        stage('Build Docker Image') {
            steps {
                script {
                    sh "docker build -t ${DOCKER_IMAGE}:${VERSION} -f LearnJenkins/Dockerfile ."
                }
            }
        }

        stage('Run Docker Container') {
            steps {
                script {
                    sh """
                    docker run -d --restart unless-stopped \\
                        -p ${HOST_PORT}:${CONTAINER_PORT} \\
                        -v ${HOST_LOG_DIR}:${CONTAINER_LOG_DIR} \\
                        ${DOCKER_IMAGE}:${VERSION}
                    """
                }
            }
        }
    }

    post {
        failure {
            echo "Deploy failed."
        }
        success {
            echo "Deploy successfully."
        }
    }
}
