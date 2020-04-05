import { User } from './User';

/**
 * Post model.
 */
export class Post {
  /*
  //access -> string
  */
  /**
   * Post Id
   * @param id number
   * Post Title
   * @param title string
   * Post Description
   * @param description string
   * Post Content
   * @param content string
   * Post Author Id
   * @param authorId number
   * Post Author
   * @param author User
   * Post Seen
   * @param seen number
   * Post Likes
   * @param likes number
   * Post Dislikes
   * @param dislikes number
   * Post Image Url
   * @param imageUrl string
   * Post Tags
   * @param tags string
   * Post Tags List
   * @param tagsList string
   * Post Comments
   * @param comments array
   * Post Comments Count
   * @param commentsCount number
   * Post Created at
   * @param createdAt string
   */
  constructor (
    public id: number,
    public title: string,
    public description: string,
    public content: string,
    public authorId: string,
    public author: User,
    public seen: number,
    public likes: number,
    public dislikes: number,
    public imageUrl: string,
    public tags: string,
    public createdAt: string,
    public commentsCount: number,
    public comments?: Comment[],
    public tagsList?: string[],
  ) {}
}
