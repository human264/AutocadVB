Imports System.Reflection
Imports System.Runtime
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.DatabaseServices
Imports Autodesk.AutoCAD.Runtime
Imports Autodesk.AutoCAD.Geometry
Imports Autodesk.AutoCAD.Colors
Imports Autodesk.AutoCAD.EditorInput

Public Class LayersClass
    <CommandMethod("DrawCirclesHor")>
    Public Sub DrawCirclesHor()
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim db As Database = doc.Database

        Using trans As Transaction = doc.TransactionManager.StartTransaction()
            Try
                doc.Editor.WriteMessage("Drawing horizontal Circles!" + vbCrLf)
                Dim bt As BlockTable
                bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead)

                Dim blockRec As ObjectId = ObjectId.Null

                If Not bt.Has("CircleBlock") Then
                    Using acBlkTblRec As New BlockTableRecord
                        acBlkTblRec.Name = "CircleBlock"

                        ' Set the insertion point for the block
                        acBlkTblRec.Origin = New Point3d(0, 0, 0)
                        acBlkTblRec.Comments = "하아아아아아아아아아아아"

                        ' Add a circle to the block
                        Using acCirc As New Circle
                            acCirc.Center = New Point3d(0, 0, 0)
                            acCirc.Radius = 2
                            acBlkTblRec.AppendEntity(acCirc)
                            trans.GetObject(db.BlockTableId, OpenMode.ForWrite)
                            bt.Add(acBlkTblRec)
                            trans.AddNewlyCreatedDBObject(acBlkTblRec, True)
                        End Using
                        
                        Dim pt1 As Point3d = New Point3d(0, 0, 0)
                        Dim pt2 As Point3d = New Point3d(25, 150, 0)
                        Dim ln As Line = New Line(pt1, pt2)
                        acBlkTblRec.AppendEntity(ln)
                        trans.AddNewlyCreatedDBObject(ln, True)
                        
                        blockRec = acBlkTblRec.Id
                    End Using
                Else
                    blockRec = bt("CircleBlock")

                End If

                If blockRec <> ObjectId.Null Then
                    Using acBlkRef As New BlockReference(New Point3d(0, 0, 0), blockRec)

                        Dim acCurSpaceBlkTblRec As BlockTableRecord
                        acCurSpaceBlkTblRec = trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite)
                        acCurSpaceBlkTblRec.AppendEntity(acBlkRef)


                        trans.AddNewlyCreatedDBObject(acBlkRef, True)
                    End Using
                End If


                trans.Commit()
            Catch ex As Exception
                doc.Editor.WriteMessage("Error encountered: " + ex.Message)
                trans.Abort()
            End Try
        End Using
    End Sub


    <CommandMethod("InsertingABlock")>
    Public Sub InsertingABlock()
        ' Get the current database and start a transaction
        Dim acCurDb As Autodesk.AutoCAD.DatabaseServices.Database
        acCurDb = Application.DocumentManager.MdiActiveDocument.Database

        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
            ' Open the Block table for read
            Dim acBlkTbl As BlockTable
            acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead)

            Dim blkRecId As ObjectId = ObjectId.Null

            If Not acBlkTbl.Has("CircleBlock") Then
                Using acBlkTblRec As New BlockTableRecord
                    acBlkTblRec.Name = "CircleBlock"

                    ' Set the insertion point for the block
                    acBlkTblRec.Origin = New Point3d(0, 0, 0)
                    acBlkTblRec.Comments = "하아아아아아아아아아아아"
                    ' Add a circle to the block
                    Using acCirc As New Circle
                        acCirc.Center = New Point3d(0, 0, 0)
                        acCirc.Radius = 2

                        acBlkTblRec.AppendEntity(acCirc)

                        acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
                        acBlkTbl.Add(acBlkTblRec)
                        acTrans.AddNewlyCreatedDBObject(acBlkTblRec, True)
                    End Using

                    blkRecId = acBlkTblRec.Id
                End Using
            Else
                blkRecId = acBlkTbl("CircleBlock")

            End If

            ' Insert the block into the current space
            If blkRecId <> ObjectId.Null Then
                Using acBlkRef As New BlockReference(New Point3d(0, 0, 0), blkRecId)

                    Dim acCurSpaceBlkTblRec As BlockTableRecord
                    acCurSpaceBlkTblRec = acTrans.GetObject(acCurDb.CurrentSpaceId, OpenMode.ForWrite)


                    acCurSpaceBlkTblRec.AppendEntity(acBlkRef)
                    acTrans.AddNewlyCreatedDBObject(acBlkRef, True)
                End Using
            End If

            ' Save the new object to the database
            acTrans.Commit()

            ' Dispose of the transaction
        End Using
    End Sub

    <CommandMethod("CreatingABlock")>
    Public Sub CreatingABlock()
        ' Get the current database and start a transaction
        Dim acCurDb As Database
        acCurDb = Application.DocumentManager.MdiActiveDocument.Database

        Using acTrans As Transaction = acCurDb.TransactionManager.StartTransaction()
            ' Open the Block table for read
            Dim acBlkTbl As BlockTable
            acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead)

            If Not acBlkTbl.Has("CircleBlock") Then
                Using acBlkTblRec As New BlockTableRecord
                    acBlkTblRec.Name = "CircleBlock"

                    ' Set the insertion point for the block
                    acBlkTblRec.Origin = New Point3d(0, 0, 0)

                    ' Add a circle to the block
                    Using acCirc As New Circle
                        acCirc.Center = New Point3d(0, 0, 0)
                        acCirc.Radius = 1000

                        acBlkTblRec.AppendEntity(acCirc)
                        acBlkTblRec.Comments = "하이"

                        acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForWrite)
                        acBlkTbl.Add(acBlkTblRec)
                        acTrans.AddNewlyCreatedDBObject(acBlkTblRec, True)
                    End Using
                End Using
            End If

            ' Save the new object to the database
            acTrans.Commit()

            ' Dispose of the transaction
        End Using
    End Sub


    <CommandMethod("ListLayers")>
    Public Sub ListLayers()

        dim doc as Document = Application.DocumentManager.MdiActiveDocument
        dim db as Database = doc.Database

        using trans as Transaction = db.TransactionManager.StartTransaction()

            dim lyTab as LayerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead)

            for Each lyID as ObjectId in lyTab
                Dim lytr as LayerTableRecord = trans.GetObject(lyID, OpenMode.ForRead)
                doc.Editor.WriteMessage(vbLf & "Layer name: " & lytr.Name)
            Next
            trans.Commit()
        End Using
    End Sub


    <CommandMethod("CreateLayer")>
    Public Sub CreateLayer()

        dim doc as Document = Application.DocumentManager.MdiActiveDocument
        dim db as Database = doc.Database

        using trans as Transaction = db.TransactionManager.StartTransaction()
            dim lyTab as LayerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead)

            'Create a new layer called Misc
            if lyTab.Has("Misc") Then
                doc.Editor.WriteMessage("Layer Already exist.")
                trans.Abort()
            Else
                lyTab.UpgradeOpen()
                dim lytr as LayerTableRecord = New LayerTableRecord()
                lytr.Name = "Misc"
                lytr.Color = color.FromColorIndex(ColorMethod.ByLayer, 1)
                lyTab.Add(lytr)
                trans.AddNewlyCreatedDBObject(lytr, True)
                db.Clayer = lyTab("Misc")
                doc.Editor.WriteMessage("Layser[" & lytr.name & "] was create successfully.")
                trans.Commit()
            End If

        End Using
    End Sub

    <CommandMethod("UpdateLayer")>
    public sub UpdateLayer()
        dim doc as Document = Application.DocumentManager.MdiActiveDocument
        dim db as Database = doc.Database

        using trans as Transaction = db.TransactionManager.StartTransaction()

            Try
                dim lyTab as LayerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead)

                'Loop through the layer collection
                for each lyID as ObjectId in lyTab
                    Dim lytr as LayerTableRecord = trans.GetObject(lyID, OpenMode.ForRead)

                    If (lytr.Name = "Misc") Then
                        'Change the Mist Layer property( Color and Linetype)
                        lytr.UpgradeOpen()
                        lytr.Color = color.FromColorIndex(colorMethod.ByLayer, 2)
                        dim ltTab as LinetypeTable = trans.GetObject(db.LinetypeTableId, OpenMode.forRead)

                        if ltTab.Has("Hidden") = True Then
                            lytr.LinetypeObjectId = ltTab("Hidden")
                        End If
                        trans.Commit()
                        doc.Editor.WriteMessage(vbLf & "Completed Updating layer: " & lytr.Name)
                        exit for
                    Else
                        doc.Editor.WriteMessage(vbLf & "Skipping Layer[" & lytr.Name & "].")
                    End If
                Next

            Catch ex as Exception

            End Try


        End Using
    End sub

    <CommandMethod("SetLayerOnOff")>
    public sub SetLayerOnOff()

        dim doc as Document = Application.DocumentManager.MdiActiveDocument
        dim db as Database = doc.Database

        using trans as Transaction = db.TransactionManager.StartTransaction()
            Try
                dim lyTab as LayerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead)
'                db.Clayer = lyTab("0")

                'Loop through the layer collection
                for each lyID as ObjectId in lyTab
                    Dim lytr as LayerTableRecord = trans.GetObject(lyID, OpenMode.ForRead)

                    If (lytr.Name = "Misc") Then
                        'Change the Mist Layer property( Color and Linetype)
                        lytr.UpgradeOpen()
                        lytr.IsOff = True

                        trans.Commit()
                        doc.Editor.WriteMessage(vbLf & "Layer: " & lytr.Name & "has been turned off.")
                        exit for
                    Else
                        doc.Editor.WriteMessage(vbLf & "Layer Not Found.")
                    End If
                Next
            Catch ex as Exception
                doc.Editor.WriteMessage("Error Encoun")
            End Try


        End Using
    End sub

    <CommandMethod("SetLayerFrozenOrThaw")>
    Public Sub SetLayerFrozenOrThaw()
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim db As Database = doc.Database

        Using trans As Transaction = db.TransactionManager.StartTransaction()

            Try
                Dim lyTab As LayerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead)
                db.Clayer = lyTab("0")

                For Each lyID As ObjectId In lyTab
                    Dim lytr As LayerTableRecord = trans.GetObject(lyID, OpenMode.ForRead)

                    If lytr.Name = "Misc" Then
                        lytr.UpgradeOpen()
                        lytr.IsFrozen = True
                        trans.Commit()
                        doc.Editor.WriteMessage(vbLf & "Layer " & lytr.Name & " has been frozen.")
                        Exit For
                    Else
                        doc.Editor.WriteMessage(vbLf & "Layer not found.")
                    End If
                Next

            Catch ex As Exception
                doc.Editor.WriteMessage("Error encountered: " & ex.Message)
                trans.Abort()
            End Try
        End Using
    End Sub

    <CommandMethod("DeleteLayer")>
    Public Sub DeleteLayer()
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        Dim db As Database = doc.Database

        Using trans As Transaction = db.TransactionManager.StartTransaction()
            Try
                Dim lyTab As LayerTable = trans.GetObject(db.LayerTableId, OpenMode.ForRead)
                db.Clayer = lyTab("0")

                For Each lyID As ObjectId In lyTab
                    Dim lytr As LayerTableRecord = trans.GetObject(lyID, OpenMode.ForRead)

                    If lytr.Name = "Misc" Then
                        lytr.UpgradeOpen()
                        lytr.Erase()
                        trans.Commit()
                        doc.Editor.WriteMessage(vbLf & "Successfully deleted Layer[" & lytr.Name & "]")

                        Exit For
                    Else
                        doc.Editor.WriteMessage(vbLf & "Layer not found.")
                    End If
                Next

            Catch ex As Exception
                doc.Editor.WriteMessage("Error encountered: " & ex.Message)
                trans.Abort()
            End Try
        End Using
    End Sub

    <CommandMethod("SetLayerToObject")>
    public sub SetlayerToObject()
        Dim doc as Document = Application.DocumentManager.MdiActiveDocument
        Dim db As Database = doc.Database

        Using trans As Transaction = db.TransactionManager.StartTransaction()
            Dim bt As BlockTable = trans.GetObject(db.BlockTableId, Openmode.ForRead)
            Dim btr As BlockTableRecord = trans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite)

            Dim pt1 As Point3d = New Point3d(0, 0, 0)
            Dim pt2 As Point3d = new Point3d(100, 100, 0)
            Dim ln As Line = new Line(pt1, pt2)
            ln.Layer = "Cabinetry"

            btr.AppendEntity(ln)
            trans.AddNewlyCreatedDBObject(ln, True)

            trans.Commit()
            doc.Editor.WriteMessage(vbLf & "New Line object was added to cabinetry layer.")


        End Using
    End sub
End Class