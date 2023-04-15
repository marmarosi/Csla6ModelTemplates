const fs = require('fs');

const versionArgs = process.argv.slice( 2 );
const projects = [
  'Contracts',
  'CslaExtensions',
  'Dal',
  'Dal.Db2',
  'Dal.Firebird',
  'Dal.MySql',
  'Dal.Oracle',
  'Dal.PostgreSql',
  'Dal.Sqlite',
  'Dal.SqlServer',
  'Endpoints',
  'EndpointTests',
  'Models',
  'Resources',
  'WebApi',
  'WebApiTests'
  ];
const appName = 'Csla6ModelTemplates';
const tmplPath = `./${appName}._/${appName}._.csproj`;
const OTag = '<Version>';
const CTag = '</Version>';
const OTLen = OTag.length;
const CTLen = CTag.length;

console.log( '' );
switch (versionArgs.length) {
  case 0:
    console.log( 'Display help: node app-version -h|--help' );
    console.log( `` );
    showVersions();
    break;
  case 1:
    if (versionArgs[ 0 ] === '-h' || versionArgs[ 0 ] === '--help') {
      console.log( 'node app-version' );
      console.log( '        Displays the version of all projects.' );
      console.log( `` );
      console.log( 'node app-version <version>' );
      console.log( '        Sets the version of all projects to <version>.' );
      console.log( `` );
      console.log( 'node app-version <project> <version>' );
      console.log( '        Sets the version of <project> to <version>.' );
      console.log( `` );
      console.log( 'node app-version major <number>|+|-' );
      console.log( '        Sets the major version of all projects to <number>,' );
      console.log( '        or increment/decrement the current major version.' );
      console.log( `` );
      console.log( 'node app-version minor <number>|+|-' );
      console.log( '        Sets the minor version of all projects to <number>,' );
      console.log( '        or increment/decrement the current minor version.' );
      console.log( `` );
      console.log( 'node app-version patch <number>|+|-' );
      console.log( '        Sets the patch version of all projects to <number>,' );
      console.log( '        or increment/decrement the current patch version.' );
      console.log( `` );
      console.log( 'node app-version -h|--help' );
      console.log( `        Displays this help.` );
    } else {
      setVersions( versionArgs[ 0 ], '' );
    }
    break;
  default:
    const arg0 = versionArgs[ 0 ];
    const arg1 = versionArgs[ 1 ];

    if (projects.includes( arg0 )) {
      setVersion( arg0, arg1 );

    } else {
      switch (arg0) {
        case 'major':
          setVersions( arg1, 'major' );
          break;
        case 'minor':
          setVersions( arg1, 'minor' );
          break;
        case 'patch':
          setVersions( arg1, 'patch' );
          break;
        default:
          console.log( `Invalid argument: ${ versionArgs[ 0 ] }` );
          console.log( 'Display help: node app-version -h|--help' );
          break;
      }
    }
    break;
}

function showVersions() {

  for (let i = 0; i < projects.length; i++) {
    showVersion( projects[ i ] );
  }
}

function showVersion(
  project
) {

  fs.readFile(
    tmplPath.replace( /_/g, project ),
    'utf8',
    ( err, contents) => {
      if (err)
        return console.log( err );

      const version = contents.substring(
        contents.indexOf( OTag ) + OTLen,
        contents.lastIndexOf( CTag )
      );
      console.log( `${ name( project + ':' ) } ${ version }` );
    } );
}

function setVersions(
  version,
  part
) {

  for (let i = 0; i < projects.length; i++) {
    setVersion( projects[ i ], version, part );
  }
}

function setVersion(
  project,
  version,
  part
) {

  const filePath = tmplPath.replace( /_/g, project );
  fs.readFile(
    filePath,
    'utf8',
    ( err, contents) => {
      if (err)
        return console.log( err );

      const start = contents.indexOf( OTag ) + OTLen;
      const end = contents.lastIndexOf( CTag );
      const current = contents.substring( start, end );
      //console.log(current);
      const updated = getVersion( current, version, part );
      const result = contents.substring( 0, start ) + updated + contents.substring( end );

      fs.writeFile(
        filePath,
        result,
        'utf8',
        err => {
          if (err)
            return console.log( err );

          showVersion( project );
        }
      )
    } );
}

function getVersion(
  current,
  version,
  part 
) {

  const first = current.indexOf( '.' );
  const last = current.lastIndexOf( '.' );

  switch (part) {
    case 'major':
      const major = current.substring( 0, first );
      return getNext( major, version ) +
        current.substring( first );
    case 'minor':
      const minor = current.substring( first + 1, last );
      return current.substring( 0, first + 1 ) +
        getNext( minor, version ) +
        current.substring( last );
    case 'patch':
      const patch = current.substring( last + 1 );
      return current.substring( 0, last + 1 ) +
        getNext( patch, version );
    default:
      return version;
  }
}

function getNext(
  current,
  version
) {

  switch (version) {
    case '+':
      return +current + 1;
    case '-':
      return Math.max( 0, +current - 1 );
    default:
      return version;
  }
}

function name(
  project
) {
	return (`${appName}.${project}`).padEnd( 36 );
}
