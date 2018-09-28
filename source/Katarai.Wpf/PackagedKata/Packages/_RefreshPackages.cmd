@echo off
SET SOLUTION_FOLDER=%1
SET PACKAGES_SOURCE_FOLDER=%1%2
SET OUT_DIR=%3
CALL %~dp0\_DoRefreshOf.cmd StringCalculator
CALL %~dp0\_DoRefreshOf.cmd FizzBuzz