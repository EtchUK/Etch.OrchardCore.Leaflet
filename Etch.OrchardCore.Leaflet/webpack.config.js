const autoprefixer = require('autoprefixer');
const FixStyleOnlyEntriesPlugin = require('webpack-fix-style-only-entries');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const path = require('path');

module.exports = {
    entry: {
        admin: './Assets/Admin/js/index.ts',
    },
    mode: 'development',
    module: {
        rules: [
            {
                enforce: 'pre',
                test: /(\.ts(x?)$)/,
                exclude: /node_modules/,
                loader: 'tslint-loader',
                options: {
                    configFile: path.join(process.cwd(), './tslint.json'),
                    emitErrors: true,
                },
            },
            {
                test: /(\.ts(x?)$)/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: 'babel-loader',
                        options: {
                            presets: [
                                [
                                    '@babel/preset-env',
                                    {
                                        corejs: { version: 3, proposals: true },
                                        useBuiltIns: 'entry',
                                    },
                                ],
                            ],
                        },
                    },
                    {
                        loader: 'ts-loader',
                        options: {
                            configFile: path.join(
                                process.cwd(),
                                './tsconfig.json'
                            ),
                        },
                    },
                ],
            },
            {
                test: /(\.js(x?)$)/,
                exclude: /node_modules/,
                use: ['babel-loader'],
            },
            {
                test: /\.css$/,
                use: ['style-loader', 'css-loader'],
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    'css-loader?-url',
                    {
                        loader: 'postcss-loader',
                        options: {
                            postcssOptions: {
                                plugins: function () {
                                    return [autoprefixer()];
                                },
                            },
                        },
                    },
                    'sass-loader',
                ],
            },
            {
                test: /\.(png|jpg)$/,
                use: {
                    loader: 'file-loader',
                    options: {
                        name: './images/[name].[ext]',
                        publicPath: '../',
                    },
                },
            },
        ],
    },
    externals: {},
    output: {
        filename: '[name].js',
        path: path.resolve(__dirname, './wwwroot/Scripts/'),
    },
    plugins: [
        new FixStyleOnlyEntriesPlugin(),
        new MiniCssExtractPlugin({
            filename: '../Styles/[name].css',
        }),
    ],
    resolve: {
        extensions: ['.js', '.css', '.ts', '.tsx'],
        alias: {
            'leaflet-css': __dirname + '/node_modules/leaflet/dist/leaflet.css',
        },
    },
};
