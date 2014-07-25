 
 module FStomp.Tests

 open NUnit.Framework
 open FsUnit
 open fstomp
 open FParsec
 
 let test p s = 
        match run p s with
        | Success(result, _, _)   -> true
        | Failure(errorMsg, _, _) -> false

 
 [<Test>]
 let ``translate works`` ()= 
    utils.translate 
        [(".", " "); (":", "zzz"); ("-", "xXx")] 
        "the.quick:brown-fox_jumped" 
        |> should equal "the quickzzzbrownxXxfox_jumped"

 [<Test>] 
 let ``fstomp doesn't parse empty header`` ()=
    test ParserInternal.sheader "" |> should be False

 [<Test>] 
 let ``fstomp doesn't parse empty header name`` ()=
    test ParserInternal.sheader ":bar" |> should be False

 [<Test>]
 let ``fstomp parses simple header`` ()=
    test ParserInternal.sheader "x:y" |> should be True    

 [<Test>]
 let ``fstomp parses empty header value`` ()=
    test ParserInternal.sheader "foo:" |> should be True

 [<Test>]
 let ``fstomp parses header with escaped characters`` ()=
    test ParserInternal.sheader "foo\tbar\"qux:\b123\t\t\'" |> should be True

 [<Test>]
 let ``fstomp parses complex header`` ()=
    test ParserInternal.sheader 
        "a !@#$%^&*()_+-=[]\\;',./<>?`~complex 738 HEADER----\\t\\t: with *(HN a complex header value" 
        |> should be True
 
 [<Test>]
 let ``fstomp parses header lists`` ()=
    test ParserInternal.sheaderList "a:b\n" |> should be True
    test ParserInternal.sheaderList "foo:123\nbar:qux\n" |> should be True
    test ParserInternal.sheaderList "foo:123\nbar:456\nqux:789\n" |> should be True

 [<Test>]
 let ``fstomp doesn't parse broken header lists`` ()=
    test ParserInternal.sheaderList "foo:123" |> should be False
    test ParserInternal.sheaderList "foo:123\nbar:456qux:789" |> should be False    
 
 [<Test>]
 let ``fstomp parses bodies`` ()=    
    test ParserInternal.sbody "" |> should be False
    test ParserInternal.sbody "simplebody" |> should be False
    test ParserInternal.sbody "this\nis\na\nbody\u0000" |> should be True    
    test ParserInternal.sbody "\u0000" |> should be True

 [<Test>]
 let ``fstomp parses payloads`` ()=    
    test ParserInternal.spayload "MESSAGE\nx:y\n\n\u0000"
        |> should be True

    test 
        ParserInternal.spayload 
        "CONNECTED\nuser-name:sdk-dotnet-originator1@test.covata.com\n\nBody\n\u0000"
        |> should be True

 [<Test>]
 let ``fstomp parses empty frames`` ()=    
    test ParserInternal.sframe "a[\"MESSAGE\n\n\u0000\"]"
        |> should be True
         
 [<Test>]
 let ``fstomp parses frames`` ()=
    test 
        ParserInternal.sframe 
        "a[\"CONNECTED\nuser-name:sdk-dotnet-originator1@test.covata.com\nheart-beat:0,0\nversion:1.2\n\nSimpleBody\n\u0000\"]"
        |> should be True

        
 [<Test>]
 let ``fstomp parses messages`` ()=    
        Parser.Parse "a[\"MESSAGE\\nabc:123\\n\\nSimpleBody\\u0000\"]"
        |> should equal { 
            ServerCommand = ServerCommand.Message
            Headers = [{ Name = "abc"; Value = "123"}]
            Body = "SimpleBody" }
 
 [<Test>]
 let ``fstomp parses real messages`` ()=     
        Parser.Parse "a[\"CONNECTED\\nuser-name:sdk-dotnet-originator1@test.covata.com\\nheart-beat:0,0\\nversion:1.2\\n\\n\\u0000\"]"
        |> should equal { 
            ServerCommand = ServerCommand.Connected
            Headers = [{ Name = "user-name"; Value = "sdk-dotnet-originator1@test.covata.com"}
                       { Name = "heart-beat"; Value = "0,0"} 
                       { Name = "version"; Value = "1.2"}]
            Body = "" }

 