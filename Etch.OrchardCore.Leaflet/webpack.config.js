const ESLintPlugin = require('eslint-webpack-plugin');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const RemoveEmptyScriptsPlugin = require('webpack-remove-empty-scripts');
const path = require('path');

module.exports = [
    {
        entry: {
            admin: './Assets/Admin/js/index.ts',
            map: './Assets/Map/js/index.ts',
        },
        output: {
            filename: '[name].js',
            path: path.resolve(__dirname, './wwwroot/Scripts/'),
        },
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
                                            corejs: {
                                                version: 3,
                                                proposals: true,
                                            },
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
                                transpileOnly: true,
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
        plugins: [new RemoveEmptyScriptsPlugin(), new ESLintPlugin()],
        resolve: {
            extensions: ['.tsx', '.ts', '.js'],
        },
        externals: {
            jquery: 'jQuery',
        },
    },
    {
        entry: {
            admin: path.join(process.cwd(), 'Assets/Admin/css/index.scss'),
            map: path.join(process.cwd(), 'Assets/Map/css/index.scss'),
        },
        output: {
            path: path.join(process.cwd(), 'wwwroot/Styles'),
        },
        module: {
            rules: [
                {
                    test: /\.(sc|c)ss$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                        {
                            loader: 'css-loader',
                            options: {
                                url: false,
                            },
                        },
                        {
                            loader: 'postcss-loader',
                            options: {
                                postcssOptions: {
                                    plugins: [['autoprefixer']],
                                },
                            },
                        },
                        'sass-loader',
                    ],
                },
            ],
        },
        plugins: [
            new MiniCssExtractPlugin({
                filename: '../Styles/[name].css',
            }),
        ],
        resolve: {
            extensions: ['.js', '.css', '.ts', '.tsx'],
            alias: {
                'leaflet-css':
                    __dirname + '/node_modules/leaflet/dist/leaflet.css',
            },
        },
    },
];
