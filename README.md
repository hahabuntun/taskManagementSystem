# This is a task management system which can be used to:
- distribute tasks
- delegate tasks
- view current project state

## It has following features:
- sprints
- presonal/project boards
- gantt chart for project
- statistics for organization/project/worker/sprint tasks
- comment section for each task
- subtaskting
- distribution of roles in a project and default roles
- file attachment to tasks

## How to run this project
- first you need to run `npm i` and `npm run build` in the VKR directory
- after that you need to go to VKRApi directory and run command `docker compose up` (takes around 2-4 minutes)
- login data can be found in file VKRApi\Vkr.Backend\Vkr.DataAccess\Configurations\WorkerConfigurations\WorkerConfiguration.cs, password is always admin for pre-existing users

## Origin - final project to get bachelor's degree
