var gulp = require('gulp');
var webpack = require('webpack');
var WebpackDevServer = require('webpack-dev-server');
var sass = require('gulp-sass');

// TODO: webpack-dev-server

gulp.task('sass', function () {
    gulp.src('./wwwroot/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./wwwroot'));
});


gulp.task('sass:watch', function () {
    gulp.watch('./wwwroot/**/*.scss', ['sass']);
});

gulp.task('default', ['webpack-dev-server', 'sass', 'sass:watch']);

gulp.task('build', ['sass', 'webpack:build']);
