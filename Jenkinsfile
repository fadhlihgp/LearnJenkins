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
                checkout scm
            }
        }

        stage('Set Environment') {
            steps {
                script {
                    if (env.BRANCH_NAME == "master") {
                        HOST_PORT = "7001"
                        DOCKER_IMAGE = "learn-jenkins-prod"
                        HOST_LOG_DIR = "/var/www/Apps/LearnJenkinsDev"
                        CONTAINER_LOG_DIR = "/var/www/Apps/LearnJenkinsDev"
                    } else if (env.BRANCH_NAME == "dev") {
                        HOST_PORT = "7002"
                    }
                    echo "Deploying branch ${env.BRANCH_NAME} ke port ${HOST_PORT}"
                }
            }
        }

        stage('Build Docker Image') {
            steps {
                script {
                    sh "docker build -t ${DOCKER_IMAGE}:${VERSION} ."
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
